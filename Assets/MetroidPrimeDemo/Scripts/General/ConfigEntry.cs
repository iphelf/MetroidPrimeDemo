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
}