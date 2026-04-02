using System.Linq;
using UnityEngine;

public class StatBlockChangesP : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    
    [SerializeField] private LevelConfigSO levelConfig;

    
    public int[] statsP = {1, 1, 1};
    
    
    private StatBlockUI statBlockUI;
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    private EnemyController enemyController;
    private Transform enemyTransform;
    
    public delegate void Up();
    public static event Up OnUp;
    
    public delegate void Down();
    public static event Down OnDown;
    
    public delegate void Error();
    public static event Error OnError;

    public delegate void DamageRefresh();
    public static event DamageRefresh OnDamageRefresh;
    
    public int PointsTotalP { get; private set; }
    public int PointsLeftP { get; private set; }

    private void OnEnable()
    {
        StatBlockInput.OnStatIncreaseP += StatIncrease;
        StatBlockInput.OnStatDecreaseP += StatDecrease;
        PlayerDamage.OnDamage += HealthDecrease;
        
    }

    private void OnDisable()
    {
        StatBlockInput.OnStatIncreaseP -= StatIncrease;
        StatBlockInput.OnStatDecreaseP -= StatDecrease;
        PlayerDamage.OnDamage -= HealthDecrease;
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        
        statBlockUI = GetComponent<StatBlockUI>();
        
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
        
        PointsTotalP = presetP.pointsTotalP;

        statsP[0] = Mathf.Max(0, presetP.playerHealth);
        
        
        
        if      (Mathf.Approximately(presetP.playerSpeed, playerStats.playerSpeedLVL0)) statsP[1] = 0;
        else if (Mathf.Approximately(presetP.playerSpeed, playerStats.playerSpeedLVL1)) statsP[1] = 1;
        else if (Mathf.Approximately(presetP.playerSpeed, playerStats.playerSpeedLVL2)) statsP[1] = 2;
        else if (Mathf.Approximately(presetP.playerSpeed, playerStats.playerSpeedLVL3)) statsP[1] = 3;
        else
        {
            Debug.LogWarning($"[StatBlockChanges] Preset speed {presetP.playerSpeed} does not match any level value; defaulting to level 1.");
            statsP[1] = 1;
        }
        
        
        if      (Mathf.Approximately(presetP.playerJump, playerStats.playerJumpLVL0)) statsP[2] = 0;
        else if (Mathf.Approximately(presetP.playerJump, playerStats.playerJumpLVL1)) statsP[2] = 1;
        else if (Mathf.Approximately(presetP.playerJump, playerStats.playerJumpLVL2)) statsP[2] = 2;
        else if (Mathf.Approximately(presetP.playerJump, playerStats.playerJumpLVL3)) statsP[2] = 3;
        else
        {
            Debug.LogWarning($"[StatBlockChanges] Preset jump {presetP.playerJump} does not match any level value; defaulting to level 1.");
            statsP[2] = 1;
        }

        Debug.Log($"[StatBlockChanges] statsP <- preset | Health:{statsP[0]} Speed Index:{statsP[1]} Jump Index:{statsP[2]}");
        

    }

    private void StatIncrease(int selectedIndex)
    {
        if (PointsLeftP > 0)
        {
            statsP[selectedIndex]++;
            OnUp?.Invoke();
            if (statsP[selectedIndex] > playerStats.aPlayerStatBounds[selectedIndex, 1])
            {
                OnError?.Invoke();
                statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 1];
            }
                    
            switch (selectedIndex)
            {
                case 0:
                    StatChangePHealth();
                    break;
                case 1:
                    StatChangePSpeed();
                    break;
                case 2:
                    StatChangePJump();
                    break;
            }
        }
        else
        {
            OnError?.Invoke();
        }
    }

    private void StatDecrease(int selectedIndex)
    {
        OnDown?.Invoke();
        statsP[selectedIndex]--;
                        
        if (statsP[selectedIndex] < playerStats.aPlayerStatBounds[selectedIndex, 0])
        {
            OnError?.Invoke();
            statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 0];
        }
                        
        switch (selectedIndex)
        {
            case 0:
                StatChangePHealth();
                break;
            case 1:
                StatChangePSpeed();
                break;
            case 2:
                StatChangePJump();
                break;
        }
    }
    

    private void RecomputePoints()
    {
        PointsLeftP = PointsTotalP - statsP.Sum();
    }
    
    public void StatChangePHealth()
    {
        playerStatsHandler.runtimeStats.playerHealth = statsP[0];
        RecomputePoints();
        statBlockUI.UpdateUI();
    }
    
    public void HealthDecrease()
    {
        statsP[0]--;
        PointsTotalP--;
        StatChangePHealth();
        OnDamageRefresh?.Invoke();
    }
    
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statsP[1]) // player speeds
            {
                
                case 0: playerStatsHandler.runtimeStats.playerSpeed = playerStats.playerSpeedLVL0; break;
                case 1: playerStatsHandler.runtimeStats.playerSpeed = playerStats.playerSpeedLVL1; break;
                case 2: playerStatsHandler.runtimeStats.playerSpeed = playerStats.playerSpeedLVL2; break;
                case 3: playerStatsHandler.runtimeStats.playerSpeed = playerStats.playerSpeedLVL3; break;
            }
            RecomputePoints();
            statBlockUI.UpdateUI();
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statsP[2]) //player jump heights
            {
                
                case 0: playerStatsHandler.runtimeStats.playerJump = playerStats.playerJumpLVL0; break;
                case 1: playerStatsHandler.runtimeStats.playerJump = playerStats.playerJumpLVL1; break;
                case 2: playerStatsHandler.runtimeStats.playerJump = playerStats.playerJumpLVL2; break;
                case 3: playerStatsHandler.runtimeStats.playerJump = playerStats.playerJumpLVL3; break;
            }
            RecomputePoints();
            statBlockUI.UpdateUI();
        }
    }

    
    

}
