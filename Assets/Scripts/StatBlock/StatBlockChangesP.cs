using System.Linq;
using UnityEngine;

public class StatBlockChangesP : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    
    [SerializeField] private LevelConfigSO levelConfig;

    
    public int[] statsP = {1, 1, 1};
    
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    private EnemyController enemyController;
    private Transform enemyTransform;
    
    
    public int iPointsTotalP;
    public int iPointsLeftP;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        
        InitializeStatsFromLevelConfig();
        
        RecomputePoints();
        
        StatChangePHealth();
        StatChangePSpeed();
        StatChangePJump();
        
    }
    
    


    private void InitializeStatsFromLevelConfig()
    {
        if (levelConfig == null)
        {
            var bootstrap = FindFirstObjectByType<LevelBootstrap>();
            if (bootstrap)
            {
                levelConfig = bootstrap.levelConfig;
            }
        }

        if (levelConfig == null || levelConfig.playerStartingPreset == null)
        {
            Debug.Log("[StatBlockChanges] No LevelConfig or no playerStartingPreset; keeping default statsP.");
            return;
        }
        
        var presetP = levelConfig.playerStartingPreset;
        
        iPointsTotalP = presetP.iPointsTotalP;

        statsP[0] = Mathf.Max(0, presetP.iPlayerHealth);
        
        
        
        if      (Mathf.Approximately(presetP.fPlayerSpeed, playerStats.playerSpeedLVL0)) statsP[1] = 0;
        else if (Mathf.Approximately(presetP.fPlayerSpeed, playerStats.playerSpeedLVL1)) statsP[1] = 1;
        else if (Mathf.Approximately(presetP.fPlayerSpeed, playerStats.playerSpeedLVL2)) statsP[1] = 2;
        else if (Mathf.Approximately(presetP.fPlayerSpeed, playerStats.playerSpeedLVL3)) statsP[1] = 3;
        else
        {
            Debug.LogWarning($"[StatBlockChanges] Preset speed {presetP.fPlayerSpeed} does not match any level value; defaulting to level 1.");
            statsP[1] = 1;
        }
        
        
        if      (Mathf.Approximately(presetP.fPlayerJump, playerStats.playerJumpLVL0)) statsP[2] = 0;
        else if (Mathf.Approximately(presetP.fPlayerJump, playerStats.playerJumpLVL1)) statsP[2] = 1;
        else if (Mathf.Approximately(presetP.fPlayerJump, playerStats.playerJumpLVL2)) statsP[2] = 2;
        else if (Mathf.Approximately(presetP.fPlayerJump, playerStats.playerJumpLVL3)) statsP[2] = 3;
        else
        {
            Debug.LogWarning($"[StatBlockChanges] Preset jump {presetP.fPlayerJump} does not match any level value; defaulting to level 1.");
            statsP[2] = 1;
        }

        Debug.Log($"[StatBlockChanges] statsP <- preset | Health:{statsP[0]} Speed Index:{statsP[1]} Jump Index:{statsP[2]}");
        

    }


    private void RecomputePoints()
    {
        iPointsLeftP = iPointsTotalP - statsP.Sum();
    }
    
    public void StatChangePHealth()
    {
        playerStatsHandler.runtimeStats.iPlayerHealth = statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statsP[1]) // player speeds
            {
                
                case 0: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL0; break;
                case 1: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL1; break;
                case 2: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL2; break;
                case 3: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL3; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statsP[2]) //player jump heights
            {
                
                case 0: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL0; break;
                case 1: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL1; break;
                case 2: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL2; break;
                case 3: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL3; break;
            }
        }
    }
    
    
    

}
