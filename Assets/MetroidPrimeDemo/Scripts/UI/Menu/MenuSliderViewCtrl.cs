using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public class MenuSliderViewCtrl : MenuEntryViewCtrl
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Slider slider;
        [SerializeField] private Button entryButton;

        public override MenuEntryViewCtrl Clone(Transform parent)
            => Clone<MenuSliderViewCtrl>(parent);

        public override void Fill(MenuEntry entry)
        {
            if (entry is not MenuSliderEntry sliderEntry) return;
            text.text = sliderEntry.EntryName;
            slider.minValue = sliderEntry.MinimumValue;
            slider.maxValue = sliderEntry.MaximumValue;
            slider.value = sliderEntry.InitialValue;
            if (sliderEntry.OnValueChanged is not null)
                slider.onValueChanged.AddListener(sliderEntry.OnValueChanged);
        }

        protected override Selectable GetSelectable() => slider;
    }
}