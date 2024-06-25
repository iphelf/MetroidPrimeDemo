﻿using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    public abstract class Ability : MonoBehaviour
    {
        [HideInInspector] public AttributeSet attributes;

        [HideInInspector] public PlayerCharacterCtrl player;

        public abstract void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig);
    }
}