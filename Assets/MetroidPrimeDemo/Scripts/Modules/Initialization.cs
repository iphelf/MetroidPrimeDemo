using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Modules
{
    public class Initialization : MonoBehaviour
    {
        [SerializeField] private GameConfig gameConfig;

        private void Awake()
        {
            var objs = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");

            if (objs.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Initialize()
        {
            Application.targetFrameRate = gameConfig.targetFrameRate;
        }
    }
}