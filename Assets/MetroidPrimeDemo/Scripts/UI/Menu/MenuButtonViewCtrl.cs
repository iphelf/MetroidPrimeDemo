using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public class MenuButtonViewCtrl : MenuEntryViewCtrl
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        public override MenuEntryViewCtrl Clone(Transform parent)
            => Clone<MenuButtonViewCtrl>(parent);

        public override void Fill(MenuEntry entry)
        {
            if (entry is not MenuButtonEntry buttonEntry) return;
            text.text = buttonEntry.EntryName;
            if (buttonEntry.Callback is not null)
                button.onClick.AddListener(buttonEntry.Callback);
        }

        protected override Selectable GetSelectable() => button;
    }
}