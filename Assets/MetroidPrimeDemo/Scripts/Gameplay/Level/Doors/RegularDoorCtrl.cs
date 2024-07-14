using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Level.Doors
{
    [SelectionBase]
    public class RegularDoorCtrl : MonoBehaviour
    {
        [SerializeField] private Aimable block;

        private void Start()
        {
            block.OnDisabled += (_, _) => Destroy(gameObject);
        }
    }
}