using Enemy;
using Player;
using Scriptable_Objects.LevelConfigs;
using StatBlock;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    // Responsible for:
    // informing stats handlers of starting stats
    // &
    // informing statBlockUI of UI position
    
    public LevelConfigSO levelConfig;
    
    [SerializeField] private PlayerStatsHandler player;

    [SerializeField] private EnemyStatsHandler enemy;

    private void Awake()
    {
        if (!player)
            player = FindFirstObjectByType<PlayerStatsHandler>();
        
        if (!enemy)
            enemy = FindFirstObjectByType<EnemyStatsHandler>();

        if (!player)
        {
            Debug.LogError("[LevelBootstrap] No PlayerStatsHandler found in scene.");
            return;
        }

        if (!levelConfig)
        {
            Debug.LogWarning("[LevelBootstrap] No LevelConfig assigned; using Player's default preset.");
            player.ResetToDefault();
            return;
        }

        if (levelConfig.playerStartingPreset)
        {
            player.ApplyPreset(levelConfig.playerStartingPreset);
            Debug.Log("[LevelBootstrap] Applied level player preset.");
        }
        else
        {
            Debug.LogWarning("[LevelBootstrap] LevelConfig has no playerStartingPreset; using default preset.");
            player.ResetToDefault();
        }
        
        

        if (!enemy)
        {
            Debug.LogError("[LevelBootstrap] No EnemyStatsHandler found in scene.");
            return;
        }

        if (levelConfig.enemyStartingPreset)
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

    private void Start()
    {
        var ui = FindFirstObjectByType<StatBlockUI>();
        if (!ui)
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