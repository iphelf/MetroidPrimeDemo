using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public class MenuInfoViewCtrl : MenuEntryViewCtrl
    {
        [SerializeField] private Button entryButton;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text infoText;

        public override MenuEntryViewCtrl Clone(Transform parent) => Clone<MenuInfoViewCtrl>(parent);

        public override void Fill(MenuEntry entry)
        {
            if (entry is not MenuInfoEntry infoEntry) return;
            nameText.text = infoEntry.EntryName;
            infoText.text = infoEntry.InfoContent;
        }

        protected override Selectable GetSelectable() => entryButton;
    }
}