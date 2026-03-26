using System.Linq;
using UnityEngine;

public class StatBlockChangesE : MonoBehaviour
{
    [SerializeField] private LevelConfigSO levelConfig;
    
    [SerializeField] private StatBlockUI statBlockUI;
    
    [SerializeField] private EnemyStats  enemyStats;
    
    [SerializeField] private Rigidbody2D enemyRb;       
    [SerializeField] private Transform enemyVisual; 
    [SerializeField] private SpriteRenderer enemyRenderer; 
    
    
    private EnemyStatsHandler enemyStatsHandler;
    public int[] statsE = {1, 1, 1};
    
    public int iPointsTotalE;
    public int iPointsLeftE;
    public delegate void Up();
    public static event Up OnUp;
    
    public delegate void Down();
    public static event Down OnDown;
    public delegate void Error();
    public static event Error OnError;
    public delegate void DamageRefresh();
    public static event DamageRefresh OnDamageRefresh;
    
    private void OnEnable()
    {
        StatBlockInput.OnStatIncreaseE += StatIncrease;
        StatBlockInput.OnStatDecreaseE += StatDecrease;
        EnemyDamage.OnDamage += HealthDecrease;
    }

    private void OnDisable()
    {
        StatBlockInput.OnStatIncreaseE -= StatIncrease;
        StatBlockInput.OnStatDecreaseE -= StatDecrease;
        EnemyDamage.OnDamage -= HealthDecrease;
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statBlockUI = GetComponent<StatBlockUI>();
        
        GameObject enemyVisual = GameObject.FindGameObjectWithTag("EnemyVisual");
        if (enemyVisual)
        {
            enemyStatsHandler = enemyVisual.GetComponent<EnemyStatsHandler>();
        }
        GameObject enemyRoot = GameObject.FindGameObjectWithTag("EnemyRoot");
        if (enemyRoot)
        {
            enemyRb = enemyRoot.GetComponent<Rigidbody2D>();
        }
        
        InitializeStatsFromLevelConfig();
        
        RecomputePoints();
        
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
        
        if (enemyVisual)
        {
            if (levelConfig == null || levelConfig.enemyStartingPreset == null)
            {
                Debug.Log("[StatBlockChanges] No LevelConfig or no enemyStartingPreset; keeping default statsE.");
                return;
            }
            
            var presetE = levelConfig.enemyStartingPreset;
            
            iPointsTotalE = presetE.iPointsTotalE;
            
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
            
            if (Mathf.Approximately(presetE.fEnemySize, enemyStats.enemySizeLVL1)) statsE[2] = 1;
            else if (Mathf.Approximately(presetE.fEnemySize, enemyStats.enemySizeLVL2)) statsE[2] = 2;
            else if (Mathf.Approximately(presetE.fEnemySize, enemyStats.enemySizeLVL3)) statsE[2] = 3;
            else
            {
                Debug.LogWarning($"[StatBlockChanges] Preset size {presetE.fEnemySize} does not match any level value; defaulting to level 1.");
                statsE[2] = 1;
            }
            Debug.Log($"[StatBlockChanges] statsE <- preset | Health:{statsE[0]} Speed Index:{statsE[1]} Size Index:{statsE[2]}");
        }

    }

    private void StatIncrease(int selectedIndex)
    {
        if (iPointsLeftE > 0)
        {
            

            statsE[selectedIndex]++;
            OnUp?.Invoke();



            if (statsE[selectedIndex] > enemyStats.aEnemyStatBounds[selectedIndex, 1])
            {
                OnError?.Invoke();
                statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 1];
            }

            switch (selectedIndex)
            {
                case 0:
                    StatChangeEHealth();
                    break;
                case 1:
                    StatChangeESpeed();
                    break;
                case 2:
                    StatChangeESize();
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
        statsE[selectedIndex]--;
                        
        if (statsE[selectedIndex] < enemyStats.aEnemyStatBounds[selectedIndex, 0])
        {
            OnError?.Invoke();
            statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 0];
        }
        
        else
        {
            OnDown?.Invoke();
        }
        
        switch (selectedIndex)
        {
            case 0:
                StatChangeEHealth();
                break;
            case 1:
                StatChangeESpeed();
                break;
            case 2:
                StatChangeESize();
                break;
        }
    }
    
    
    private void RecomputePoints()
    {
        iPointsLeftE = iPointsTotalE - statsE.Sum();
    }

    public void StatChangeEHealth()
    {
        if (!enemyRb || !enemyVisual) return;
        
        enemyStatsHandler.runtimeStats.iEnemyHealth = statsE[0];
        RecomputePoints();
        statBlockUI.UpdateUI();
    }
    
    public void HealthDecrease()
    {
        statsE[0]--;
        iPointsTotalE--;
        StatChangeEHealth();
        OnDamageRefresh?.Invoke();
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
        RecomputePoints();
        statBlockUI.UpdateUI();
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
        RecomputePoints();
        statBlockUI.UpdateUI();
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
