using System.Collections;
using DG.Tweening;
using MetroidPrimeDemo.Scripts.Gameplay.EnemyAI;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace MetroidPrimeDemo.Scripts.Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EnemyVision))]
    [RequireComponent(typeof(EnemyHearing))]
    public class BeetleCtrlOld : EnemyCharacterCtrl
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float chaseStoppingDistance = 2.5f;
        [SerializeField] private float rotationSpeed = 180.0f;
        [SerializeField] private float maxBashDistance = 4.0f;
        [SerializeField] private float bashDuration = 0.3f;

        private NavMeshAgent _agent;
        private Rigidbody _body;
        private EnemyVision _vision;
        private EnemyHearing _hearing;

        private StateMachine<State> _fsm;
        private Vector3 _lastPositionSensed = Vector3.positiveInfinity;

        private enum State
        {
            Patrol,
            Chase,
            Search,
            Attack,
            Dying,
        }

        protected override void Start()
        {
            base.Start();

            _agent = GetComponent<NavMeshAgent>();
            _body = GetComponent<Rigidbody>();
            _vision = GetComponent<EnemyVision>();
            _vision.SetTarget(Player.transform);
            _hearing = GetComponent<EnemyHearing>();
            _hearing.SetTarget(Player.transform);

            _fsm = new StateMachine<State>();

            _fsm.AddState(State.Patrol);
            _fsm.AddState(State.Chase, new CoState<State>(this, ChaseRoutine, loop: false));
            _fsm.AddState(State.Search, new CoState<State>(this, SearchRoutine, loop: false));
            _fsm.AddState(State.Attack, new CoState<State>(this, AttackRoutine, loop: false, needsExitTime: true,
                onEnter: AttackEnter, onExit: AttackExit));
            _fsm.AddState(State.Dying, new CoState<State>(this, DieRoutine, loop: false));
            _fsm.SetStartState(State.Patrol);

            _fsm.AddTransition(State.Patrol, State.Chase, _ => _vision.CanSee || _hearing.CanHear);
            _fsm.AddTransition(State.Search, State.Chase, _ => _vision.CanSee || _hearing.CanHear);
            _fsm.AddTransition(State.Attack, State.Search, _ => !_vision.CanSee && !_hearing.CanHear);
            _fsm.AddTransition(State.Attack, State.Chase,
                _ => Vector3.Distance(transform.position, _lastPositionSensed) > maxBashDistance);

            _fsm.Init();

            _lastPositionSensed = transform.position;
        }

        private void Update()
        {
            if (_vision.CanSee)
                _lastPositionSensed = _vision.LastPositionSeen;
            else if (_hearing.CanHear)
                _lastPositionSensed = _hearing.LastPositionHeard;

            _fsm.OnLogic();
        }

        private IEnumerator ChaseRoutine()
        {
            Debug.Log(nameof(ChaseRoutine));

            while (true)
            {
                if (!_vision.CanSee && !_hearing.CanHear)
                    break;

                _agent.SetDestination(_lastPositionSensed);
                _agent.stoppingDistance = chaseStoppingDistance;
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                    break;

                yield return new WaitForSeconds(0.1f);
            }

            if (_vision.CanSee || _hearing.CanHear)
                _fsm.RequestStateChange(State.Attack);
            else
                _fsm.RequestStateChange(State.Search);
        }

        private IEnumerator MoveTowards(Vector3 position)
        {
            _agent.SetDestination(position);
            _agent.stoppingDistance = 0.0f;
            while (_agent.remainingDistance > _agent.stoppingDistance)
                yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator SearchRoutine()
        {
            yield return MoveTowards(_lastPositionSensed);
            yield return new WaitForSeconds(4.0f);
            _fsm.RequestStateChange(State.Patrol);
        }

        private void AttackEnter(CoState<State, string> state)
        {
            _agent.ResetPath();
            _agent.enabled = false;
        }

        private IEnumerator AttackRoutine()
        {
            Debug.Log(nameof(AttackRoutine));

            Vector3 bashDirection;

            // 1. Turn
            while (true)
            {
                if (_fsm.HasPendingTransition)
                {
                    _fsm.StateCanExit();
                    yield break;
                }

                bashDirection = _lastPositionSensed - transform.position;
                bashDirection = Vector3.ProjectOnPlane(bashDirection, transform.up);
                bashDirection.Normalize();
                if (Mathf.Approximately(Vector3.Angle(transform.forward, bashDirection), 0.0f))
                    break;

                Quaternion targetRotation = Quaternion.LookRotation(bashDirection);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. Windup
            // Animation
            yield return new WaitForSeconds(0.7f);
            Vector3 bashTarget = transform.position + bashDirection * maxBashDistance;

            // 3. Bash
            // Animation
            yield return _body.DOMove(bashTarget, bashDuration).WaitForCompletion();

            // 4. Recovery
            yield return new WaitForSeconds(0.5f);

            // _fsm.RequestStateChange(State.Chase, forceInstantly: true);
            _fsm.StateCanExit();
        }

        private void AttackExit(CoState<State, string> state)
        {
            _agent.enabled = true;
            _agent.Warp(transform.position);
        }

        protected override void OnDamaged()
        {
            _fsm.RequestStateChange(State.Dying, forceInstantly: true);
        }

        private IEnumerator DieRoutine()
        {
            _agent.ResetPath();
            _agent.enabled = false;
            animator?.Play("Die");
            OnDisable();
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }
    }
}