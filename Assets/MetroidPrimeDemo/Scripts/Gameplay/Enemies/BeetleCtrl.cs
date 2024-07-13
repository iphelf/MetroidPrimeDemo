using System.Collections;
using DG.Tweening;
using MetroidPrimeDemo.Scripts.Gameplay.EnemyAI;
using MetroidPrimeDemo.Scripts.General;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace MetroidPrimeDemo.Scripts.Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EnemyVision))]
    [RequireComponent(typeof(EnemyHearing))]
    public class BeetleCtrl : EnemyCharacterCtrl
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PatrolWaypoints patrolWaypoints;
        [SerializeField] private float chaseStoppingDistance = 3.8f;
        [SerializeField] private float rotationSpeed = 180.0f;
        [SerializeField] private float maxBashDistance = 4.0f;
        [SerializeField] private float bashDuration = 0.3f;
        [SerializeField] private TriggerListener bodyHitbox;
        [SerializeField] private float bodyDamage = 10.0f;
        [SerializeField] private TriggerListener bashHitbox;
        [SerializeField] private float bashDamage = 25.0f;

        private NavMeshAgent _agent;
        private Rigidbody _body;
        private EnemyVision _vision;
        private EnemyHearing _hearing;

        private StateMachine<State> _fsm;
        private HybridStateMachine<State> _attackFsm;

        private Vector3 _lastPositionSensed = Vector3.positiveInfinity;
        [SerializeField, ReadOnly] private bool sensed;
        [SerializeField, ReadOnly] private bool inRange;

        private enum State
        {
            Patrol,
            Search,
            Chase,
            Attack,
            Aim,
            Telegraph,
            Bash,
            Die,
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

            bodyHitbox.OnTriggerEnterEvent.AddListener(OnBodyHit);
            bashHitbox.OnTriggerEnterEvent.AddListener(OnBashHit);
            bashHitbox.gameObject.SetActive(false);

            _fsm = new StateMachine<State>();
            _attackFsm = new HybridStateMachine<State>(
                needsExitTime: true,
                beforeOnEnter: _ =>
                {
                    _agent.ResetPath();
                    _agent.enabled = false;
                },
                afterOnExit: _ =>
                {
                    _agent.enabled = true;
                    _agent.Warp(transform.position);
                }
            );

            _fsm.AddState(State.Patrol, new CoState<State>(this, PatrolRoutine, loop: true));
            _fsm.AddState(State.Search, new CoState<State>(this, SearchRoutine, loop: false));
            _fsm.AddState(State.Chase, new CoState<State>(this, ChaseRoutine, loop: true));
            _fsm.AddState(State.Die, new CoState<State>(this, DieRoutine, loop: false));
            _attackFsm.AddState(State.Aim,
                onEnter: _ => animator?.Play("None"),
                onLogic: _ => AimUpdate()
            );
            _attackFsm.AddState(State.Telegraph, new CoState<State>(this,
                TelegraphRoutine, needsExitTime: true, loop: false));
            _attackFsm.AddState(State.Bash, new CoState<State>(this,
                BashRoutine, needsExitTime: true, loop: false));
            _fsm.AddState(State.Attack, _attackFsm);

            _fsm.SetStartState(State.Patrol);
            _fsm.AddTransition(State.Patrol, State.Chase, _ => sensed);
            _fsm.AddTwoWayTransition(State.Search, State.Chase, _ => sensed);
            _fsm.AddTwoWayTransition(State.Chase, State.Attack, _ => inRange);
            _attackFsm.SetStartState(State.Aim);
            _attackFsm.AddExitTransition(State.Aim);
            _attackFsm.AddExitTransition(State.Telegraph);
            _attackFsm.AddExitTransition(State.Bash);

            _fsm.Init();

            _lastPositionSensed = transform.position;
        }

        private void Update()
        {
            if (_vision.CanSee)
                _lastPositionSensed = _vision.LastPositionSeen;
            else if (_hearing.CanHear)
                _lastPositionSensed = _hearing.LastPositionHeard;
            sensed = _vision.CanSee || _hearing.CanHear;
            inRange = sensed && Vector3.Distance(transform.position, _lastPositionSensed) <= maxBashDistance;

            _fsm.OnLogic();
        }

        private IEnumerator MoveTowards(Vector3 position, float stoppingDistance)
        {
            _agent.SetDestination(position);
            _agent.stoppingDistance = stoppingDistance;
            while (_agent.remainingDistance > _agent.stoppingDistance)
                yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator PatrolRoutine()
        {
            animator?.Play("Patrol");
            int waypointIndex = patrolWaypoints.NearestWaypoint(transform.position, out var waypoint);
            while (_fsm.ActiveStateName == State.Patrol)
            {
                yield return MoveTowards(waypoint, 0.5f);
                yield return new WaitForSeconds(1.0f);
                waypointIndex = patrolWaypoints.NextWaypoint(waypointIndex, out waypoint);
            }
        }

        private IEnumerator ChaseRoutine()
        {
            animator?.Play("Chase");
            do
            {
                _agent.SetDestination(_lastPositionSensed);
                _agent.stoppingDistance = chaseStoppingDistance;
                yield return new WaitForSeconds(0.3f);
            } while (_agent.remainingDistance > _agent.stoppingDistance);
        }

        private IEnumerator SearchRoutine()
        {
            animator?.Play("Search");
            yield return MoveTowards(_lastPositionSensed, 0.5f);
            yield return new WaitForSeconds(4.0f);
            _fsm.RequestStateChange(State.Patrol);
        }

        private void AimUpdate()
        {
            if (_attackFsm.ActiveStateName != State.Aim) return;

            var bashDirection = _lastPositionSensed - transform.position;
            bashDirection = Vector3.ProjectOnPlane(bashDirection, transform.up);
            bashDirection.Normalize();

            if (Mathf.Approximately(Vector3.Angle(transform.forward, bashDirection), 0.0f))
            {
                _attackFsm.RequestStateChange(State.Telegraph);
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(bashDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private IEnumerator TelegraphRoutine()
        {
            animator?.Play("Telegraph");
            yield return new WaitForSeconds(0.7f);
            if (!_attackFsm.HasPendingTransition)
                _attackFsm.RequestStateChange(State.Bash);
            _attackFsm.StateCanExit();
        }

        private IEnumerator BashRoutine()
        {
            animator?.Play("Bash");
            Vector3 bashTarget = transform.position + transform.forward * maxBashDistance;
            bashHitbox.gameObject.SetActive(true);
            yield return _body.DOMove(bashTarget, bashDuration).WaitForCompletion();
            bashHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            if (!_attackFsm.HasPendingTransition)
                _attackFsm.RequestStateChange(State.Aim);
            _attackFsm.StateCanExit();
        }

        private void OnBashHit(Collider other)
        {
            if (other.GetComponent<PlayerCharacterCtrl>() != Player)
            {
                return;
            }

            Player.Hurt(bashDamage);
        }

        private void OnBodyHit(Collider other)
        {
            if (other.GetComponent<PlayerCharacterCtrl>() != Player)
                return;
            Player.Hurt(bodyDamage);
        }

        protected override void OnDamaged()
        {
            _fsm.RequestStateChange(State.Die, forceInstantly: true);
        }

        private IEnumerator DieRoutine()
        {
            bodyHitbox.gameObject.SetActive(false);
            bashHitbox.gameObject.SetActive(false);
            _agent.ResetPath();
            _agent.enabled = false;
            animator?.Play("Die");
            OnDisable();
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }
    }
}