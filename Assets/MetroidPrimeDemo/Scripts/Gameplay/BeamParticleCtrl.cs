using UnityEngine;
using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BeamParticleCtrl : MonoBehaviour
    {
        public readonly UnityEvent<GameObject> OnDamage = new();

        private void OnParticleCollision(GameObject other)
        {
            OnDamage.Invoke(other);
        }
    }
}