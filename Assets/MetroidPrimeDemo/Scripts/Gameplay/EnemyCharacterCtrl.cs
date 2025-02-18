﻿using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [SelectionBase]
    public class EnemyCharacterCtrl : Aimable
    {
        public float maxHealth;
        private float _health;
        public float HealthRatio => maxHealth == 0.0f ? 0.0f : _health / maxHealth;
        protected PlayerCharacterCtrl Player;
        private bool _destroying;
        public EventHandler OnKill;

        protected virtual void Start()
        {
            _health = maxHealth;

            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterCtrl>();
        }

        public void DealDamage(float damage)
        {
            if (_destroying) return;

            _health = Mathf.Clamp(_health - damage, 0.0f, maxHealth);
            if (_health == 0.0f)
            {
                _destroying = true;
                OnKill?.Invoke(this, null);
                OnDamaged();
            }
        }

        protected virtual void OnDamaged()
        {
            Destroy(gameObject);
        }
    }
}