using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{
    public PlayerStatsPresetSO defaultPreset;
    public PlayerRuntimeStats runtimeStats;


    void Start()
    {
        
    }
    
    public void ApplyPreset(PlayerStatsPresetSO preset)
    {
        var source = preset != null ? preset : defaultPreset;
        if (source == null)
        {
            Debug.LogWarning("[PlayerStatsHandler] No preset provided and no default preset set.");
            runtimeStats = new PlayerRuntimeStats();
            return;
        }

        runtimeStats = source.CreateRuntimeCopy();
    }

    public void ResetToDefault() => ApplyPreset(defaultPreset);
}