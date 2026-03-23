// LevelBootstrap.cs
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    public LevelConfigSO levelConfig;
    
    public PlayerStatsHandler player;

    public EnemyStatsHandler enemy;
    void Awake()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerStatsHandler>();
        
        if (enemy == null)
            enemy = FindFirstObjectByType<EnemyStatsHandler>();

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
        
        

        if (enemy == null)
        {
            Debug.LogError("[LevelBootstrap] No EnemyStatsHandler found in scene.");
            return;
        }

        if (levelConfig.enemyStartingPreset != null)
        {
            enemy.ApplyPreset(levelConfig.enemyStartingPreset);
            Debug.Log("[LevelBootstrap] Applied level enemy preset.");
        }
        else
        {
            Debug.LogWarning("[LevelBootstrap] LevelConfig has no enemyStartingPreset; using default preset.");
            enemy.ResetToDefault();
        }
    }

    void Start()
    {
        var ui = FindFirstObjectByType<StatBlockUI>();
        if (ui == null)
        {
            
            Debug.LogWarning("[LevelBootstrap] No StatBlockUI found.");
            return;
        }
        StartCoroutine(ApplyUIWhenReady(ui));
    }

    private System.Collections.IEnumerator ApplyUIWhenReady(StatBlockUI ui)
    {
        yield return null;
        
        Debug.Log("[LevelBootstrap] Applying UI layout from LevelConfig...");
        
        ui.ApplyLevelUILayout(levelConfig);
        
    }
    
    
}