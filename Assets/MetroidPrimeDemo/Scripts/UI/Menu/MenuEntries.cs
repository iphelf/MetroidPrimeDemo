using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public abstract class MenuEntry
    {
        public string EntryName = string.Empty;
    }

    public class MenuButtonEntry : MenuEntry
    {
        public UnityAction Callback;
    }

    public class MenuInfoEntry : MenuEntry
    {
        public string InfoContent = string.Empty;
    }

    public class MenuSliderEntry : MenuEntry
    {
        public float MinimumValue = 0.0f;
        public float MaximumValue = 1.0f;
        public Binding<float> Binding = null;
    }
}