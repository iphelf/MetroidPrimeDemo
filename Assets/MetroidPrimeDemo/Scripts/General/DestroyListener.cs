using UnityEngine;
using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.General
{
    public class DestroyListener : MonoBehaviour
    {
        public readonly UnityEvent<GameObject> OnDestroyEvent = new();

        private void OnDestroy()
        {
            OnDestroyEvent.Invoke(gameObject);
        }
    }
}