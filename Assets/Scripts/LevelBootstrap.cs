// LevelBootstrap.cs
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    public LevelConfigSO levelConfig;
    public PlayerStatsHandler player;

    void Awake()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerStatsHandler>();

        if (player == null)
        {
            Debug.LogError("[LevelBootstrap] No PlayerStatsHandler found in scene.");
            return;
        }

        if (levelConfig == null)
        {
            Debug.LogWarning("[LevelBootstrap] No LevelConfig assigned; using Player's default preset.");
            player.ResetToDefault();
            return;
        }

        if (levelConfig.playerStartingPreset != null)
        {
            player.ApplyPreset(levelConfig.playerStartingPreset);
            Debug.Log("[LevelBootstrap] Applied level player preset.");
        }
        else
        {
            Debug.LogWarning("[LevelBootstrap] LevelConfig has no playerStartingPreset; using default preset.");
            player.ResetToDefault();
        }
    }
}