using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Starting Stats/Player Stats Preset", fileName = "NewPlayerStatsPreset")]
    public class PlayerStatsPresetSO : ScriptableObject
    {
    
        [Header("Starting Stat Levels")]
        public StatLevel healthLevel = StatLevel.Level1;
        public StatLevel speedLevel  = StatLevel.Level1;
        public StatLevel jumpLevel   = StatLevel.Level1;
    
        [Header("Total Points")]
        public int pointsTotal = 5;
    
    }
}
