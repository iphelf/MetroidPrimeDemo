using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public class MenuSliderViewCtrl : MenuEntryViewCtrl
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text minText;
        [SerializeField] private TMP_Text maxText;
        [SerializeField] private Button entryButton;
        private Binding<float> _binding;

        private void Start()
        {
            slider.onValueChanged.AddListener(OnSetBySlider);
        }

        private void OnDestroy()
        {
            ClearBinding();
        }

        public override MenuEntryViewCtrl Clone(Transform parent)
            => Clone<MenuSliderViewCtrl>(parent);

        private void ClearBinding()
        {
            if (_binding is null) return;
            _binding.OnSetBySource.RemoveListener(OnSetBySource);
            _binding = null;
        }

        private void SetBinding(Binding<float> binding)
        {
            if (binding is null) return;
            _binding = binding;
            slider.value = _binding.Get();
            _binding.OnSetBySource.AddListener(OnSetBySource);
        }

        private void OnSetBySource(float value) => slider.value = value;

        private void OnSetBySlider(float value) => _binding?.Set(value);

        public override void Fill(MenuEntry entry)
        {
            if (entry is not MenuSliderEntry sliderEntry) return;
            text.text = sliderEntry.EntryName;
            slider.minValue = sliderEntry.MinimumValue;
            slider.maxValue = sliderEntry.MaximumValue;
            minText.text = sliderEntry.MinimumValue.ToString("N0");
            maxText.text = sliderEntry.MaximumValue.ToString("N0");
            ClearBinding();
            SetBinding(sliderEntry.Binding);
        }

        protected override Selectable GetSelectable() => slider;
    }
}