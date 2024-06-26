using System;

namespace MetroidPrimeDemo.Scripts.General
{
    [Serializable]
    public class ConfigEntry
    {
        public string key = "";
        public string value = "";
        private string Name => key;
    }

    [Serializable]
    public class ConfigEntry<T>
    {
        public string key = "";
        public T value = default;
        private string Name => key;
    }
}