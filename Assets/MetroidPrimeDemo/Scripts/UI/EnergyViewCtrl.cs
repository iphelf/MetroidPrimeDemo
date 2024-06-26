using MetroidPrimeDemo.Scripts.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI
{
    public class EnergyViewCtrl : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentEnergyTankText;
        [SerializeField] private Slider currentEnergyTankSlider;
        [SerializeField] private Transform energyTankContainer;
        [SerializeField] private GameObject energyTankTemplate;
        [SerializeField] private PlayerCharacterCtrl player;
        private int _lastHealth;

        private void Update()
        {
            int health = Mathf.CeilToInt(player.attributes.health);
            if (health == _lastHealth) return;

            int currentEnergyTank = health % 100;
            currentEnergyTankText.text = currentEnergyTank.ToString("D2");
            currentEnergyTankSlider.value = currentEnergyTank;

            int energyTank = health / 100;
            int energyTankDiff = energyTank - (energyTankContainer.childCount - 1);
            if (energyTankDiff > 0)
            {
                for (int i = 0; i < energyTankDiff; ++i)
                {
                    var go = Instantiate(energyTankTemplate, energyTankContainer);
                    go.SetActive(true);
                }
            }
            else if (energyTankDiff < 0)
            {
                for (int i = energyTankContainer.childCount - 1; i > energyTank; --i)
                {
                    var go = energyTankContainer.GetChild(i).gameObject;
                    Destroy(go);
                }
            }

            _lastHealth = health;
        }
    }
}