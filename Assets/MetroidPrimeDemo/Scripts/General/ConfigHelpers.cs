using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.General
{
    public static class ConfigHelpers
    {
        private class ObjectVisitor : PropertyVisitor, IVisitPropertyAdapter<float>, IVisitPropertyAdapter<GameObject>
        {
            private static readonly ObjectVisitor Instance = new();
            private Dictionary<string, string> _configs;
            private Dictionary<string, GameObject> _deps;

            private ObjectVisitor() => AddAdapter(this);

            protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property,
                ref TContainer container, ref TValue value)
            {
                // traverse direct fields only (not nested fields)
            }

            public void Visit<TContainer>(
                in VisitContext<TContainer, float> context,
                ref TContainer container,
                ref float value)
            {
                if (Instance._configs is not null
                    && Instance._configs.Remove(context.Property.Name, out var valueString))
                    float.TryParse(valueString, out value);
            }

            public void Visit<TContainer>(in VisitContext<TContainer, GameObject> context, ref TContainer container,
                ref GameObject value)
            {
                if (Instance._deps is not null
                    && Instance._deps.Remove(context.Property.Name, out var gameObject))
                    value = gameObject;
            }

            public static void OverrideFields<T>(List<ConfigEntry> configs, T obj)
            {
                Instance._configs = configs.ToDictionary(config => config.key, config => config.value);
                PropertyContainer.Accept(Instance, ref obj);
                foreach (var (key, value) in Instance._configs)
                    Debug.LogWarning($"ConfigEntry `{key}` cannot be applied to {obj}");
                Instance._configs = null;
            }

            public static void OverrideFields<T>(List<ConfigEntry<GameObject>> deps, T obj)
            {
                Instance._deps = deps.ToDictionary(config => config.key, config => config.value);
                PropertyContainer.Accept(Instance, ref obj);
                foreach (var (key, value) in Instance._deps)
                    Debug.LogWarning($"ConfigEntry `{key}` cannot be applied to {obj}");
                Instance._deps = null;
            }
        }

        public static void ApplySerializableConfigs<T>(List<ConfigEntry> configs, T obj)
        {
            ObjectVisitor.OverrideFields(configs, obj);
        }

        public static void ApplySerializableConfigs<T>(List<ConfigEntry<GameObject>> configs, T obj)
        {
            ObjectVisitor.OverrideFields(configs, obj);
        }
    }
}