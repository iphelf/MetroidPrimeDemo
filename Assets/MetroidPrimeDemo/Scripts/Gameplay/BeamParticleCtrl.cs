using UnityEngine;
using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BeamParticleCtrl : MonoBehaviour
    {
        public readonly UnityEvent<Aimable> OnDamage = new();

        private void OnParticleCollision(GameObject other)
        {
            if (Aimable.IsValid(other, out var aimable))
                OnDamage.Invoke(aimable);
        }
    }
}