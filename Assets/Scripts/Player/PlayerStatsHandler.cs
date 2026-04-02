using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{
    
    [SerializeField] private PlayerStatValues statValues;
    [SerializeField] private PlayerStatsPresetSO defaultPreset;

    public PlayerRuntimeStats runtimeStats;

    
    
    public void ApplyPreset(PlayerStatsPresetSO preset)
    {
        if (preset == null || statValues == null)
        {
            Debug.LogWarning("[PlayerStatsHandler] Missing preset or stats table.");
            return;
        }

        runtimeStats.playerHealth = statValues.healthByLevel[(int)preset.healthLevel];
        runtimeStats.playerSpeed  = statValues.speedByLevel[(int)preset.speedLevel];
        runtimeStats.playerJump   = statValues.jumpByLevel[(int)preset.jumpLevel];
    }


    public void ResetToDefault() => ApplyPreset(defaultPreset);
}