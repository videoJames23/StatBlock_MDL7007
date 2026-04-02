using System.Linq;
using UnityEngine;

public class StatBlockChangesE : MonoBehaviour
{
    [SerializeField] private LevelConfigSO levelConfig;
    [SerializeField] private EnemyStatValues  enemyStats;
    
    [SerializeField] private StatBlockUI statBlockUI;
    
    [SerializeField] private Rigidbody2D enemyRb;       
    [SerializeField] private Transform enemyVisual; 
    [SerializeField] private SpriteRenderer enemyRenderer; 
    private EnemyStatsHandler enemyStatsHandler;
    
    public int[] statsE = {1, 1, 1};
    
    public int PointsTotalE{ get; private set; }
    public int PointsLeftE{ get; private set; }
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
        
        var enemyVisualGO = GameObject.FindGameObjectWithTag("EnemyVisual");
        if (enemyVisualGO)
        {
            enemyVisual = enemyVisualGO.transform;
            enemyStatsHandler = enemyVisualGO.GetComponent<EnemyStatsHandler>();
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

        if (levelConfig == null || levelConfig.enemyStartingPreset == null)
        {
            Debug.Log("[StatBlockChanges] No LevelConfig or no enemyStartingPreset; keeping default statsE.");
            return;
        }
        
        var presetE = levelConfig.enemyStartingPreset;
        
        statsE[0] = (int)presetE.healthLevel;
        statsE[1] = (int)presetE.speedLevel;
        statsE[2] = (int)presetE.sizeLevel;

        PointsTotalE = presetE.pointsTotal;
        RecomputePoints();

        Debug.Log($"[StatBlockChanges] statsE <- preset | Health:{statsE[0]} Speed Index:{statsE[1]} Jump Index:{statsE[2]}");

    }
    private void RecomputePoints()
    {
        PointsLeftE = PointsTotalE - statsE.Sum();
    }

    private void StatIncrease(int selectedIndex)
    {
        if (PointsLeftE > 0)
        {
            

            statsE[selectedIndex]++;
            OnUp?.Invoke();



            if (statsE[selectedIndex] > enemyStats.enemyStatBounds[selectedIndex, 1])
            {
                OnError?.Invoke();
                statsE[selectedIndex] = enemyStats.enemyStatBounds[selectedIndex, 1];
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
                        
        if (statsE[selectedIndex] < enemyStats.enemyStatBounds[selectedIndex, 0])
        {
            OnError?.Invoke();
            statsE[selectedIndex] = enemyStats.enemyStatBounds[selectedIndex, 0];
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
    

    private void StatChangeEHealth()
    {
        if (!enemyRb || !enemyVisual) return;
        
        enemyStatsHandler.runtimeStats.enemyHealth = enemyStats.healthByLevel[statsE[0]];
        RecomputePoints();
        statBlockUI.UpdateUI();
    }

    private void HealthDecrease()
    {
        statsE[0]--;
        PointsTotalE--;
        StatChangeEHealth();
        OnDamageRefresh?.Invoke();
    }

    private void StatChangeESpeed()
    {
        if (!enemyRb || !enemyVisual) return;

        enemyStatsHandler.runtimeStats.enemySpeed = enemyStats.speedByLevel[statsE[1]];
        RecomputePoints();
        statBlockUI.UpdateUI();
    }


    private void StatChangeESize()
    {
        if (!enemyRb || !enemyVisual) return;

        var newScale = enemyStats.sizeByLevel[statsE[2]];

        enemyStatsHandler.runtimeStats.enemySize = newScale;
        ApplyEnemyScaleBottomAnchored(newScale);
        RecomputePoints();
        statBlockUI.UpdateUI();
    }


    
    private void ApplyEnemyScaleBottomAnchored(float scale)
    {
        var rootPosition = enemyRb.position;
        
        var localScale = enemyVisual.localScale;
        enemyVisual.localScale = new Vector3(scale, scale, localScale.z);
        
        var spriteHeight = GetSpriteHeightUnits(enemyRenderer);
        var childLocalY = (spriteHeight * scale) * 0.5f;
        var localPosition = enemyVisual.localPosition;
        enemyVisual.localPosition = new Vector3(localPosition.x, childLocalY, localPosition.z);
        
        enemyRb.position = rootPosition;
        
    }
    
}
