using System.Linq;
using Enemy;
using Player;
using Scriptable_Objects.LevelConfigs;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace StatBlock
{
    public class StatBlockChangesP : MonoBehaviour
    {
        [SerializeField] private PlayerStatValues  playerStats;
    
        [SerializeField] private LevelConfigSO levelConfig;
        [SerializeField] private LevelBootstrap levelBootstrap;

    
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

        private int PointsTotalP { get; set; }
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
        private void Start()
        {
            levelBootstrap = FindFirstObjectByType<LevelBootstrap>();
            levelConfig = levelBootstrap.levelConfig;
        
            var player = GameObject.FindGameObjectWithTag("Player");
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
            
            statsP[0] = (int)presetP.healthLevel;
            statsP[1] = (int)presetP.speedLevel;
            statsP[2] = (int)presetP.jumpLevel;

            PointsTotalP = presetP.pointsTotal;
            RecomputePoints();

            Debug.Log($"[StatBlockChanges] statsP <- preset | Health:{statsP[0]} Speed Index:{statsP[1]} Jump Index:{statsP[2]}");
            
        }

        private void StatIncrease(int selectedIndex)
        {
            if (PointsLeftP > 0)
            {
                statsP[selectedIndex]++;
                OnUp?.Invoke();
                if (statsP[selectedIndex] > playerStats.PlayerStatBounds[selectedIndex, 1])
                {
                    OnError?.Invoke();
                    statsP[selectedIndex] = playerStats.PlayerStatBounds[selectedIndex, 1];
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
                        
            if (statsP[selectedIndex] < playerStats.PlayerStatBounds[selectedIndex, 0])
            {
                OnError?.Invoke();
                statsP[selectedIndex] = playerStats.PlayerStatBounds[selectedIndex, 0];
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

        private void StatChangePHealth()
        {
            if (!playerController) return;
            playerStatsHandler.runtimeStats.playerHealth = playerStats.healthByLevel[statsP[0]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }

        private void HealthDecrease()
        {
            if (!playerController) return;
            statsP[0]--;
            PointsTotalP--;
            StatChangePHealth();
            OnDamageRefresh?.Invoke();
        }

        private void StatChangePSpeed()
        {
            if (!playerController) return;
            playerStatsHandler.runtimeStats.playerSpeed = playerStats.speedByLevel[statsP[1]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }

        private void StatChangePJump()
        {
            if (!playerController) return;
            playerStatsHandler.runtimeStats.playerJump  = playerStats.jumpByLevel[statsP[2]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }
    }
}
