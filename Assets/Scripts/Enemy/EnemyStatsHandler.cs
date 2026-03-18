using UnityEngine;

public class EnemyStatsHandler : MonoBehaviour
{
    public EnemyStatsPresetSO defaultPreset;
    public EnemyRuntimeStats runtimeStats;


    void Start()
    {
        
    }
    
    public void ApplyPreset(EnemyStatsPresetSO preset)
    {
        var source = preset != null ? preset : defaultPreset;
        if (source == null)
        {
            Debug.LogWarning("[EnemyStatsHandler] No preset provided and no default preset set.");
            runtimeStats = new EnemyRuntimeStats();
            return;
        }

        runtimeStats = source.CreateRuntimeCopy();
    }

    public void ResetToDefault() => ApplyPreset(defaultPreset);
}