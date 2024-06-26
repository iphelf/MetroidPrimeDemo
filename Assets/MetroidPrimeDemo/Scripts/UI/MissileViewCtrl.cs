using MetroidPrimeDemo.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.UI
{
    public class MissileViewCtrl : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentMissileText;
        [SerializeField] private TMP_Text maxMissileText;
        [SerializeField] private PlayerCharacterCtrl player;
        private int _lastCurrentMissile;
        private int _lastMaxMissile;

        private void Update()
        {
            int currentMissile = player.attributes.missiles;
            if (currentMissile != _lastCurrentMissile)
                currentMissileText.text = currentMissile.ToString("D3");
            _lastCurrentMissile = currentMissile;

            int maxMissile = player.attributes.maxMissiles;
            if (maxMissile != _lastMaxMissile)
                maxMissileText.text = maxMissile.ToString("D3");
            _lastMaxMissile = maxMissile;
        }
    }
}