using System.Collections.Generic;
using DG.Tweening;
using MetroidPrimeDemo.Scripts.UI.Menu;
using TMPro;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.UI
{
    public class MenuViewCtrl : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Transform menuListRoot;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private MenuInfoViewCtrl infoTemplate;
        [SerializeField] private MenuButtonViewCtrl buttonTemplate;
        [SerializeField] private MenuSliderViewCtrl sliderTemplate;

        private readonly List<MenuEntryViewCtrl> _entryCtrls = new();

        private void ClearEntries()
        {
            foreach (var entryCtrl in _entryCtrls)
                Destroy(entryCtrl.gameObject);
            _entryCtrls.Clear();
        }

        public void FocusFirstEntry()
        {
            if (_entryCtrls.Count > 0)
                _entryCtrls[0].Focus();
        }

        private void SetEntries(string title, List<MenuEntry> entries)
        {
            titleText.text = title;
            foreach (var entry in entries)
            {
                MenuEntryViewCtrl template = entry switch
                {
                    MenuButtonEntry => buttonTemplate,
                    MenuInfoEntry => infoTemplate,
                    MenuSliderEntry => sliderTemplate,
                    _ => null
                };
                if (template is null) continue;
                MenuEntryViewCtrl instance = template.Clone(menuListRoot);
                instance.Fill(entry);
                _entryCtrls.Add(instance);
            }

            for (int i = 0, n = _entryCtrls.Count; i < n; ++i)
            {
                MenuEntryViewCtrl prev = _entryCtrls[(i - 1 + n) % n];
                MenuEntryViewCtrl curr = _entryCtrls[i];
                MenuEntryViewCtrl next = _entryCtrls[(i + 1) % n];
                curr.LinkNavigation(prev, next);
            }

            FocusFirstEntry();
        }

        public void FillEntries(string title, List<MenuEntry> entries, bool fadeOut = true, bool fadeIn = true) =>
            StartCoroutine(FillEntriesRoutine(title, entries, fadeOut, fadeIn));

        private async Awaitable FillEntriesRoutine(
            string title, List<MenuEntry> entries, bool fadeOut = true, bool fadeIn = true)
        {
            if (fadeOut)
                await canvasGroup.DOFade(0.0f, 0.15f).AsyncWaitForCompletion();

            ClearEntries();
            SetEntries(title, entries);

            if (fadeIn)
                await canvasGroup.DOFade(1.0f, 0.15f).AsyncWaitForCompletion();
        }
    }
}