using System;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class MissileCtrl : MonoBehaviour
    {
        [SerializeField] private float speed = 15.0f;
        [SerializeField] private float rotationSpeed = 180.0f;
        [SerializeField] private float explosionRadius = 0.6f;
        [SerializeField] private GameObject missileObject;
        [SerializeField] private GameObject explosionObject;
        [SerializeField] private TriggerListener triggerListener;
        private bool _flying;
        private Aimable _target;
        private float _lifetime;
        private Action<Aimable> _onDamage;

        private void Start()
        {
            triggerListener.OnTriggerEnterEvent.AddListener(OnTrigger);
            missileObject.SetActive(true);
            explosionObject.SetActive(false);
        }

        private void OnDestroy()
        {
            triggerListener.OnTriggerEnterEvent.RemoveListener(OnTrigger);
        }

        public void Launch(Aimable target, float lifetime, Action<Aimable> onDamage)
        {
            _flying = true;
            _target = target;
            _lifetime = lifetime;
            _onDamage = onDamage;
            if (_target is not null)
                _target.OnDisabled += OnAimableDisabled;
        }

        private void OnAimableDisabled(object sender, EventArgs _)
        {
            if (sender is not Aimable aimable) return;
            aimable.OnDisabled -= OnAimableDisabled;
            _target = null;
        }

        private void Update()
        {
            if (!_flying) return;

            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0.0f)
            {
                StartCoroutine(Explode());
                return;
            }

            if (_target is not null)
            {
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(_target.transform.position - transform.position),
                    rotationSpeed * Time.deltaTime
                );
            }

            transform.position += speed * Time.deltaTime * transform.forward;
        }

        private void OnTrigger(Collider other)
        {
            StartCoroutine(Explode());
        }

        private async Awaitable Explode()
        {
            _flying = false;
            missileObject.SetActive(false);

            explosionObject.SetActive(true);
            Collider[] hitColliders = new Collider[10];
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position, explosionRadius, hitColliders,
                Aimable.LayerMask
            );
            for (int i = 0; i < hitCount; ++i)
                if (Aimable.IsValid(hitColliders[i].gameObject, out var aimable))
                    _onDamage(aimable);
            await Awaitable.WaitForSecondsAsync(1.0f);

            Destroy(gameObject);
        }
    }
}