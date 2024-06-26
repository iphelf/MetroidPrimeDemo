using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Modules
{
    public class DynamicRoot : MonoBehaviour
    {
        private static Transform _root;

        private void OnEnable()
        {
            _root = transform;
        }

        private void OnDisable()
        {
            _root = null;
        }

        public static GameObject Instantiate(GameObject prefab, Transform transform)
        {
            var go = Instantiate(prefab, transform.position, transform.rotation, _root);
            return go;
        }
    }
}