using System.Collections.Generic;
using System.Linq;
using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay;
using MetroidPrimeDemo.Scripts.Modules;
using MetroidPrimeDemo.Scripts.UI.Menu;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.UI
{
    public class PauseViewCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private MenuViewCtrl menu;
        [SerializeField] private InputConfig inputConfig;
        [SerializeField] private PlayerCharacterCtrl player;

        private bool _isPaused;

        private readonly string _pauseMenuTitle = "Pause Menu";
        private List<MenuEntry> _pauseMenuEntries;
        private readonly string _optionsMenuTitle = "Options";
        private List<MenuEntry> _optionsMenuEntries;
        private readonly string _abilitiesMenuTitle = "Abilities";

        private class BgmVolumeBinding : Binding<float>
        {
            public override float Get() => AudioMgr.BgmVolume;
            public override void Set(float value) => AudioMgr.BgmVolume = value;
        }

        private class SfxVolumeBinding : Binding<float>
        {
            public override float Get() => AudioMgr.SfxVolume;
            public override void Set(float value) => AudioMgr.SfxVolume = value;
        }


        private void Start()
        {
            _pauseMenuEntries = new List<MenuEntry>
            {
                new MenuButtonEntry { EntryName = "Resume", Callback = ResumeGame },
                new MenuButtonEntry
                {
                    EntryName = "Abilities",
                    Callback = () => menu.FillEntries(_abilitiesMenuTitle, CollectAbilityEntries())
                },
                new MenuButtonEntry
                {
                    EntryName = "Options",
                    Callback = () => menu.FillEntries(_optionsMenuTitle, _optionsMenuEntries)
                },
                new MenuButtonEntry
                {
                    EntryName = "Return to Title",
                    Callback = () =>
                    {
                        Time.timeScale = 1.0f;
                        GameFlow.ReturnToMainMenu();
                    }
                },
                new MenuButtonEntry
                {
                    EntryName = "Quit",
                    Callback = GameFlow.QuitGame
                },
            };
            _optionsMenuEntries = new List<MenuEntry>
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
                new MenuButtonEntry
                    { EntryName = "Back", Callback = () => menu.FillEntries(_pauseMenuTitle, _pauseMenuEntries) },
            };

            panel.SetActive(false);
        }

        private void Update()
        {
            if (inputConfig.action.WasPressedThisFrame())
            {
                if (_isPaused) ResumeGame();
                else PauseGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0.0f;

            panel.SetActive(true);
            menu.FillEntries(_pauseMenuTitle, _pauseMenuEntries, fadeOut: false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _isPaused = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1.0f;

            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _isPaused = false;
        }

        private List<MenuEntry> CollectAbilityEntries()
        {
            List<MenuEntry> entries = player.abilities.Abilities
                .Select(abilityConfig => new MenuInfoEntry
                {
                    EntryName = abilityConfig.data.displayName,
                    InfoContent = abilityConfig.data.description
                })
                .Cast<MenuEntry>().ToList();

            entries.Add(new MenuButtonEntry
            {
                EntryName = "Back",
                Callback = () => menu.FillEntries(_pauseMenuTitle, _pauseMenuEntries),
            });

            return entries;
        }
    }
}