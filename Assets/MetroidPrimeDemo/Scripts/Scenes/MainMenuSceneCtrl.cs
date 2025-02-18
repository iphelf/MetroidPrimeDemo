﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Modules;
using MetroidPrimeDemo.Scripts.UI;
using MetroidPrimeDemo.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Scenes
{
    public class MainMenuSceneCtrl : MonoBehaviour
    {
        [SerializeField] private CanvasGroup titlePanel;
        [SerializeField] private InputConfig startInputConfig;
        [SerializeField] private CanvasGroup menuPanel;
        [SerializeField] private MenuViewCtrl menu;

        private InputAction _startInput;
        private CanvasGroup _currentPanel;

        private readonly string _mainMenuTitle = "Main Menu";
        private readonly string _optionsMenuTitle = "Options";

        private void Start()
        {
            _startInput = startInputConfig.data.FindAction();

            FadeIn(titlePanel);

            var optionsButtonInMainMenu = new MenuButtonEntry { EntryName = _optionsMenuTitle, Callback = null };
            var mainMenuEntries = new List<MenuEntry>
            {
                new MenuButtonEntry { EntryName = "New Game", Callback = GameFlow.NewGame },
                optionsButtonInMainMenu,
                new MenuButtonEntry { EntryName = "Quit", Callback = GameFlow.QuitGame },
                new MenuButtonEntry { EntryName = "Back", Callback = () => CrossFade(menuPanel, titlePanel) },
            };
            var optionsMenuEntries = new List<MenuEntry>
            {
                new MenuSliderEntry
                {
                    EntryName = "BGM Volume",
                    MinimumValue = 0.0f, MaximumValue = 100.0f,
                    Binding = new BgmVolumeBinding(),
                },
                new MenuSliderEntry
                {
                    EntryName = "SFX Volume",
                    MinimumValue = 0.0f, MaximumValue = 100.0f,
                    Binding = new SfxVolumeBinding(),
                },
                new MenuSliderEntry
                {
                    EntryName = "Target Frame Rate",
                    MinimumValue = 24.0f, MaximumValue = 144.0f,
                    Binding = new TargetFrameRateBinding(),
                },
                new MenuButtonEntry
                    { EntryName = "Back", Callback = () => menu.FillEntries(_mainMenuTitle, mainMenuEntries) },
            };
            optionsButtonInMainMenu.Callback = () => menu.FillEntries(_optionsMenuTitle, optionsMenuEntries);

            menu.FillEntries(_mainMenuTitle, mainMenuEntries, fadeOut: false, fadeIn: false);
            menuPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_currentPanel == titlePanel)
                if (_startInput.WasPressedThisFrame())
                {
                    menu.FocusFirstEntry();
                    CrossFade(titlePanel, menuPanel);
                }
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