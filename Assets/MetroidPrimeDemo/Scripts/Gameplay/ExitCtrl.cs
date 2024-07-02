using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class ExitCtrl : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerCharacterCtrl>();
                if (player)
                    GameFlow.FinishGame(player);
            }
        }
    }
}