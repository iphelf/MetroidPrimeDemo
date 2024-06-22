using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Data
{
    [Serializable]
    public class AbilityEntry
    {
        public bool enabled = true;
        public InputConfig input;
        public AbilityConfig ability;
    }

    [CreateAssetMenu(menuName = "Scriptable Object/Player Config", fileName = "player")]
    public class PlayerConfig : ScriptableObject
    {
        public float gravity = 20.0f;
        public List<AbilityEntry> initialAbilities = new();
    }
}