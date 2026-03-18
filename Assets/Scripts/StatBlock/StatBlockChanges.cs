using System.Linq;
using UnityEngine;

public class StatBlockChanges : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    
    [SerializeField] private Rigidbody2D enemyRb;       
    [SerializeField] private Transform enemyVisual; 
    [SerializeField] private SpriteRenderer enemyRenderer; 
    
    [SerializeField] private LevelConfigSO levelConfig;

    
    public int[] statsP = {1, 1, 1};
    public int[] statsE = {1, 1, 1};
    
    private StatBlockInput statBlockInput;
    private StatBlockUI statBlockUI;
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    private EnemyController enemyController;
    private Transform enemyTransform;
    private EnemyStatsHandler enemyStatsHandler;
    
    public int iPointsTotalP;
    public int iPointsLeftP;
    public int iPointsTotalE;
    public int iPointsLeftE;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockUI = GetComponent<StatBlockUI>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        
        GameObject enemyVisual = GameObject.FindGameObjectWithTag("EnemyVisual");
        if (enemyVisual)
        {
            enemyController = enemyVisual.GetComponent<EnemyController>();
            enemyTransform = enemyVisual.GetComponent<Transform>();
            enemyStatsHandler = enemyVisual.GetComponent<EnemyStatsHandler>();
        }
        GameObject enemyRoot = GameObject.FindGameObjectWithTag("EnemyRoot");
        if (enemyRoot)
        {
            enemyRb = enemyRoot.GetComponent<Rigidbody2D>();
        }
        
        InitializeStatsFromLevelConfig();
        
        RecomputePoints();
        
        StatChangePHealth();
        StatChangePSpeed();
        StatChangePJump();
        StatChangeEHealth();
        StatChangeESpeed();
        StatChangeESize();

        
        
        
        
    }
    
    private float GetSpriteHeightUnits(SpriteRenderer sr)
    {
        if (!sr || sr.sprite == null) return 1f;
        return sr.sprite.bounds.size.y;
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

        if (enemyVisual)
        {
            var presetE = levelConfig.enemyStartingPreset;
            
            statsE[0] = Mathf.Max(0, presetE.iEnemyHealth);
            
            if      (Mathf.Approximately(presetE.fEnemySpeed, enemyStats.enemySpeedLVL0)) statsE[1] = 0;
            else if (Mathf.Approximately(presetE.fEnemySpeed, enemyStats.enemySpeedLVL1)) statsE[1] = 1;
            else if (Mathf.Approximately(presetE.fEnemySpeed, enemyStats.enemySpeedLVL2)) statsE[1] = 2;
            else if (Mathf.Approximately(presetE.fEnemySpeed, enemyStats.enemySpeedLVL3)) statsE[1] = 3;
            else
            {
                Debug.LogWarning($"[StatBlockChanges] Preset speed {presetE.fEnemySpeed} does not match any level value; defaulting to level 1.");
                statsE[1] = 1;
            }
            
            if (Mathf.Approximately(presetE.fEnemySpeed, enemyStats.enemySpeedLVL1)) statsE[1] = 1;
            else if (Mathf.Approximately(presetE.fEnemySize, enemyStats.enemySizeLVL2)) statsE[1] = 2;
            else if (Mathf.Approximately(presetE.fEnemySize, enemyStats.enemySizeLVL3)) statsE[1] = 3;
            else
            {
                Debug.LogWarning($"[StatBlockChanges] Preset speed {presetE.fEnemySize} does not match any level value; defaulting to level 1.");
                statsE[2] = 1;
            }
        }

    }


    private void RecomputePoints()
    {
        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
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
    
    
    public void StatChangeEHealth()
    {
        if (!enemyRb || !enemyVisual) return;
        
        enemyStatsHandler.runtimeStats.iEnemyHealth = statsE[0];
        
    }
    public void StatChangeESpeed()
    {
            if (!enemyRb || !enemyVisual) return;

            float newSpeed = statsE[1] switch
            {
                0 => enemyStats.enemySpeedLVL0,
                1 => enemyStats.enemySpeedLVL1,
                2 => enemyStats.enemySpeedLVL2,
                3 => enemyStats.enemySpeedLVL3,
                _ => enemyRb.linearVelocity.x
            };

            enemyStatsHandler.runtimeStats.fEnemySpeed = newSpeed;
    }
    
    public void StatChangeESize()
    {
        if (!enemyRb || !enemyVisual) return;

        float newScale = statsE[2] switch
        {
            1 => enemyStats.enemySizeLVL1,
            2 => enemyStats.enemySizeLVL2,
            3 => enemyStats.enemySizeLVL3,
            _ => enemyVisual.localScale.x
        };

        enemyStatsHandler.runtimeStats.fEnemySize = newScale;
        ApplyEnemyScaleBottomAnchored(newScale);
    }

    
    private void ApplyEnemyScaleBottomAnchored(float scale)
    {
        Vector2 rootPosition = enemyRb.position;
        
        var localScale = enemyVisual.localScale;
        enemyVisual.localScale = new Vector3(scale, scale, localScale.z);
        
        float spriteHeight = GetSpriteHeightUnits(enemyRenderer);
        float childLocalY = (spriteHeight * scale) * 0.5f;
        var localPosition = enemyVisual.localPosition;
        enemyVisual.localPosition = new Vector3(localPosition.x, childLocalY, localPosition.z);
        
        enemyRb.position = rootPosition;
        
    }

}
