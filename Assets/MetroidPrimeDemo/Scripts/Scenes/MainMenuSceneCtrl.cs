using System;
using DG.Tweening;
using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.Scenes
{
    public class MainMenuSceneCtrl : MonoBehaviour
    {
        [Header("Title Panel")] [SerializeField]
        private CanvasGroup titlePanel;

        private InputAction startInput;

        [Header("Main Menu Panel")] [SerializeField]
        private CanvasGroup mainMenuPanel;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button backFromMainMenuButton;

        [Header("Options Panel")] [SerializeField]
        private CanvasGroup optionsPanel;

        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Button backFromOptionsButton;

        private CanvasGroup _currentPanel;

        private void Start()
        {
            startInput = InputSystem.actions.FindAction("Player/FireBeam");
            FadeIn(titlePanel);

            mainMenuPanel.gameObject.SetActive(false);
            newGameButton.onClick.AddListener(GameFlow.NewGame);
            optionsButton.onClick.AddListener(() => CrossFade(mainMenuPanel, optionsPanel));
            quitButton.onClick.AddListener(Application.Quit);
            backFromMainMenuButton.onClick.AddListener(() => CrossFade(mainMenuPanel, titlePanel));

            optionsPanel.gameObject.SetActive(false);
            sfxVolumeSlider.onValueChanged.AddListener(AudioMgr.SetSfxVolume);
            bgmVolumeSlider.onValueChanged.AddListener(AudioMgr.SetBgmVolume);
            backFromOptionsButton.onClick.AddListener(() => CrossFade(optionsPanel, mainMenuPanel));
        }

        private void Update()
        {
            if (_currentPanel == titlePanel)
                if (startInput.WasPressedThisFrame())
                    CrossFade(titlePanel, mainMenuPanel);
        }

        private void FadeIn(CanvasGroup canvasGroup)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0.0f;
            canvasGroup.DOFade(1.0f, 0.25f);
            _currentPanel = canvasGroup;
        }

        private static void FadeOut(CanvasGroup canvasGroup, Action onComplete = null)
        {
            canvasGroup.DOFade(0.0f, 0.25f).OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }

        private void CrossFade(CanvasGroup current, CanvasGroup next)
            => FadeOut(current, () => FadeIn(next));
    }
}