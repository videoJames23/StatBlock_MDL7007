using Scriptable_Objects;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Enemy
{
    public class EnemyStatsHandler : MonoBehaviour
    {
        [SerializeField] private EnemyStatValues statValues;
        [SerializeField] private EnemyStatsPresetSO defaultPreset;

        public EnemyRuntimeStats runtimeStats;
        
        public void ApplyPreset(EnemyStatsPresetSO preset)
        {
            if (!preset || !statValues)
            {
                Debug.LogWarning("[EnemyStatsHandler] Missing preset or stats table.");
                return;
            }

            runtimeStats.enemyHealth = statValues.healthByLevel[(int)preset.healthLevel];
            runtimeStats.enemySpeed  = statValues.speedByLevel[(int)preset.speedLevel];
            runtimeStats.enemySize = statValues.sizeByLevel[(int)preset.sizeLevel];
        
            runtimeStats.enemyDir = preset.enemyDir;
        }


        public void ResetToDefault() => ApplyPreset(defaultPreset);
    }
}