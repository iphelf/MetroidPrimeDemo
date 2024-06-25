using System;
using System.Collections.Generic;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Data
{
    public enum AbilityType
    {
        None = 0,
        Jump = 1,
        Move = 2,
        Look = 3,
        FireBeam = 4,
        Aim = 5,
        FireMissile = 6,
        ChargeBeam = 7,

        Custom = 99,
    }

    [Serializable]
    public class AbilityConfigData
    {
        public AbilityType type = AbilityType.None;
        public List<ConfigEntry> config = new();

        public bool TryReadConfig(string key, out string value)
        {
            var entry = config.Find(entry => entry.key == key);
            if (entry == null)
            {
                value = null;
                return false;
            }
            else
            {
                value = entry.value;
                return true;
            }
        }
    }

    [CreateAssetMenu(menuName = "Scriptable Object/Ability Config", fileName = "ability")]
    public class AbilityConfig : ScriptableObject
    {
        public AbilityConfigData data = new();
    }
}