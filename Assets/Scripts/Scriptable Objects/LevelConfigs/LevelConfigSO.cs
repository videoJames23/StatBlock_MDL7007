using UnityEngine;

namespace Scriptable_Objects.LevelConfigs
{
    [CreateAssetMenu(menuName = "Levels/Level Config", fileName = "NewLevelConfig")]
    public class LevelConfigSO : ScriptableObject
    {
        // Responsible for:
        // compiling starting stats and UI position
        // &
        // making them available for LevelBootstrap
        
        // Starting stats for player
        [Header("Player")]
        public PlayerStatsPresetSO playerStartingPreset;

        // Starting stats for enemy
        [Header("Enemy")] 
        public EnemyStatsPresetSO enemyStartingPreset;
    
        // Out-of-focus position for StatBlock UI
        [Header("StatBlock UI (preview layout)")]
        public Vector2 uiHolderAnchoredPosition = new (-601.17f, -373.82f);
        public Vector3 uiHolderScale            = new (1f, 1f, 1f);
        public Vector3 uiBackgroundScale        = new (2.83f, 2.31f, 1f);
    }
}
