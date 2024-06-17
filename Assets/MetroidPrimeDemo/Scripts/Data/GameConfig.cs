using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Data
{
    [CreateAssetMenu(menuName = "Scriptable Object/Game Config", fileName = "game")]
    public class GameConfig : ScriptableObject
    {
        public int targetFrameRate = 120;
    }
}