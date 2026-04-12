using System.Linq;
using Enemy;
using Scriptable_Objects.LevelConfigs;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace StatBlock
{
    public class StatBlockChangesE : MonoBehaviour
    {
        // Responsible for:
        // initialising and changing enemy stats
        // &
        // tracking available points
        // &
        // resizing enemy sprite
        
        [Header("Stats")]
        [SerializeField] private LevelConfigSO levelConfig;
        [SerializeField] private EnemyStatValues  enemyStats;
    
        [Header("UI")]
        [SerializeField] private StatBlockUI statBlockUI;
        
        [Header("Enemy")]
        [SerializeField] private Rigidbody2D enemyRb;       
        [SerializeField] private Transform enemyVisualTransform; 
        [SerializeField] private SpriteRenderer enemyRenderer; 
        [SerializeField] private EnemyStatsHandler enemyStatsHandler;
    
        public int[] statsE = {1, 1, 1};

        private int PointsTotalE{ get; set; }
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
        private void Start()
        {
            InitializeStatsFromLevelConfig();
        
            RecomputePoints();
        
            StatChangeEHealth();
            StatChangeESpeed();
            StatChangeESize();
        }
    
    
        private static float GetSpriteHeightUnits(SpriteRenderer sr)
        {
            if (!sr || !sr.sprite) return 1f;
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
                
                if (statsE[selectedIndex] > enemyStats.EnemyStatBounds[selectedIndex, 1])
                {
                    OnError?.Invoke();
                    statsE[selectedIndex] = enemyStats.EnemyStatBounds[selectedIndex, 1];
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
                        
            if (statsE[selectedIndex] < enemyStats.EnemyStatBounds[selectedIndex, 0])
            {
                OnError?.Invoke();
                statsE[selectedIndex] = enemyStats.EnemyStatBounds[selectedIndex, 0];
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
            if (!enemyRb || !enemyVisualTransform) return;
        
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
            if (!enemyRb || !enemyVisualTransform) return;

            enemyStatsHandler.runtimeStats.enemySpeed = enemyStats.speedByLevel[statsE[1]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }


        private void StatChangeESize()
        {
            if (!enemyRb || !enemyVisualTransform) return;
            
            enemyStatsHandler.runtimeStats.enemySize = enemyStats.sizeByLevel[statsE[2]];
            var newScale = enemyStatsHandler.runtimeStats.enemySize;
            
            ApplyEnemyScaleBottomAnchored(newScale);
            RecomputePoints();
            statBlockUI.UpdateUI();
        }


    
        private void ApplyEnemyScaleBottomAnchored(float scale)
        {
            var spriteHeight = GetSpriteHeightUnits(enemyRenderer);
            var childLocalY = (spriteHeight * scale) * 0.5f;
            
            var localScale = enemyVisualTransform.localScale;
            var localPosition = enemyVisualTransform.localPosition;
            
            var rootPosition = enemyRb.position;
            
            enemyVisualTransform.localScale = new Vector3(scale, scale, localScale.z);
            enemyVisualTransform.localPosition = new Vector3(localPosition.x, childLocalY, localPosition.z);
            enemyRb.position = rootPosition;
        
        }
    
    }
}
