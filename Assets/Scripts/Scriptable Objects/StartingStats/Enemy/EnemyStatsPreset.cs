using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Starting Stats/Enemy Stats Preset", fileName = "NewEnemyStatsPreset")]
    public class EnemyStatsPresetSO : ScriptableObject
    {
        [Header("Starting Stat Levels")]
        public StatLevel healthLevel = StatLevel.Level1;
        public StatLevel speedLevel  = StatLevel.Level1;
        public StatLevel sizeLevel   = StatLevel.Level1;
    
        [Header ("Starting Direction: +1 == right, -1 == left")]
        public int enemyDir = 1;
    
        [Header("Total Points")]
        public int pointsTotal = 5;
    
    }
}