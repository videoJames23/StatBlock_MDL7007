using UnityEngine;

public class EnemyStatsHandler : MonoBehaviour
{
    [SerializeField] private EnemyStatValues statValues;
    [SerializeField] private EnemyStatsPresetSO defaultPreset;

    public EnemyRuntimeStats runtimeStats;

    
    
    public void ApplyPreset(EnemyStatsPresetSO preset)
    {
        if (preset == null || statValues == null)
        {
            Debug.LogWarning("[EnemyStatsHandler] Missing preset or stats table.");
            return;
        }

        runtimeStats.enemyHealth = statValues.healthByLevel[(int)preset.healthLevel];
        runtimeStats.enemySpeed  = statValues.speedByLevel[(int)preset.speedLevel];
        
        
        int sizeLevel = Mathf.Clamp
        (
            (int)preset.sizeLevel,
            0,
            statValues.sizeByLevel.Length - 1
        );

        runtimeStats.enemySize = statValues.sizeByLevel[sizeLevel];
        
        runtimeStats.enemyDir = preset.enemyDir;
    }


    public void ResetToDefault() => ApplyPreset(defaultPreset);
}