using MetroidPrimeDemo.Scripts.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.Scenes
{
    public class GameOverSceneCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject successPanel;
        [SerializeField] private TMP_Text percentageCompleteText;
        [SerializeField] private TMP_Text totalTimeText;

        [SerializeField] private GameObject failurePanel;

        [SerializeField] private Button retryButton;
        [SerializeField] private Button giveUpButton;

        private void Start()
        {
            var outcome = GameFlow.Outcome;
            percentageCompleteText.text = $"{outcome.Completion * 100:N0}%";
            totalTimeText.text = $"{outcome.Time / 60:00}:{outcome.Time % 60.0f:00}";
            successPanel.SetActive(outcome.Success);
            failurePanel.SetActive(!outcome.Success);

            retryButton.onClick.AddListener(GameFlow.RetryLevel);
            giveUpButton.onClick.AddListener(GameFlow.ReturnToMainMenu);
        }
    }
}