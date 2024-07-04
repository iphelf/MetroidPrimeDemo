using System;
using System.Collections.Generic;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Data
{
    public enum InputType
    {
        None,
        Direction,
        DirectionDelta,
        Button,
    }

    [Serializable]
    public class InputConfigData
    {
        [SerializeField] private InputActionAsset actionsAsset;
        public InputActionAsset ActionsAsset => actionsAsset == null ? InputSystem.actions : actionsAsset;
        public string action;
        public InputType type = InputType.None;
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

        public InputAction FindAction() => ActionsAsset.FindAction(action);
    }

    [CreateAssetMenu(menuName = "Scriptable Object/Input Config", fileName = "input")]
    public class InputConfig : ScriptableObject
    {
        public InputConfigData data;
    }
}