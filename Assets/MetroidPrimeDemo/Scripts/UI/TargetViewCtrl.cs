using MetroidPrimeDemo.Scripts.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI
{
    public class TargetViewCtrl : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private PlayerCharacterCtrl player;

        private string _lastTargetName;
        private float? _lastHealth;

        private void Start()
        {
            canvasGroup.alpha = 0.0f;
        }

        private void Update()
        {
            Aimable target = player.attributes.lockTarget;
            canvasGroup.alpha = !target ? 0.0f : 1.0f;

            string targetName = target?.targetName;
            if (targetName != _lastTargetName && targetName is not null)
                nameText.text = targetName;

            float? health = (target as EnemyCharacterCtrl)?.HealthRatio;
            if (health.HasValue)
            {
                if (!_lastHealth.HasValue)
                    healthBar.gameObject.SetActive(true);
                if (!_lastHealth.HasValue || !Mathf.Approximately(_lastHealth.Value, health.Value))
                    healthBar.value = health.Value;
            }
            else if (_lastHealth.HasValue)
                healthBar.gameObject.SetActive(false);

            _lastTargetName = targetName;
            _lastHealth = health;
        }
    }
}