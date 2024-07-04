using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public abstract class Binding<T>
    {
        public abstract T Get();
        public abstract void Set(T value);
        public readonly UnityEvent<T> OnSetBySource = new();
    }
}