﻿using UnityEngine;
using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.General
{
    [RequireComponent(typeof(Collider))]
    public class TriggerListener : MonoBehaviour
    {
        public readonly UnityEvent<Collider> OnTriggerEnterEvent = new();

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent.Invoke(other);
        }
    }
}