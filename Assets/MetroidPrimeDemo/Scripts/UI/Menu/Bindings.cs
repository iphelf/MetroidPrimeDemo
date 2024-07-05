using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public abstract class Binding<T>
    {
        public abstract T Get();
        public abstract void Set(T value);
        public readonly UnityEvent<T> OnSetBySource = new();
    }

    public class BgmVolumeBinding : Binding<float>
    {
        public override float Get() => AudioMgr.BgmVolume;
        public override void Set(float value) => AudioMgr.BgmVolume = value;
    }

    public class SfxVolumeBinding : Binding<float>
    {
        public override float Get() => AudioMgr.SfxVolume;
        public override void Set(float value) => AudioMgr.SfxVolume = value;
    }

    public class TargetFrameRateBinding : Binding<float>
    {
        public override float Get() => Application.targetFrameRate;
        public override void Set(float value) => Application.targetFrameRate = Mathf.RoundToInt(value);
    }
}