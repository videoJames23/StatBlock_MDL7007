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
        // Responsible for:
        // initialising and changing player stats
        // &
        // tracking available points
        
        [Header("Stats")]
        [SerializeField] private LevelConfigSO levelConfig;
        [SerializeField] private PlayerStatValues  playerStats;
    
        [Header("UI")]
        [SerializeField] private StatBlockUI statBlockUI;
    
        [Header("Player")]
        [SerializeField] private PlayerStatsHandler playerStatsHandler;
    
        public int[] statsP = {1, 1, 1};
        
        private int PointsTotalP { get; set; }
        public int PointsLeftP { get; private set; }
        
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
            InitializeStatsFromLevelConfig();
        
            RecomputePoints();
        
            StatChangePHealth();
            StatChangePSpeed();
            StatChangePJump();
        }
        
        // Sets starting stats on player
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

        // Increases selected stat
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

        // Decreases selected stat
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
    

        // Recomputes how many points the player has left
        private void RecomputePoints()
        {
            PointsLeftP = PointsTotalP - statsP.Sum();
        }

        // Changes player health through StatBlock input
        private void StatChangePHealth()
        {
            if (!playerStatsHandler) return;
            playerStatsHandler.runtimeStats.playerHealth = playerStats.healthByLevel[statsP[0]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }

        // Decreases player health on damage
        private void HealthDecrease()
        {
            if (!playerStatsHandler) return;
            statsP[0]--;
            PointsTotalP--;
            StatChangePHealth();
            OnDamageRefresh?.Invoke();
        }

        // Changes player speed through StatBlock input
        private void StatChangePSpeed()
        {
            if (!playerStatsHandler) return;
            playerStatsHandler.runtimeStats.playerSpeed = playerStats.speedByLevel[statsP[1]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }
        
        // Changes player jump height through StatBlock input
        private void StatChangePJump()
        {
            if (!playerStatsHandler) return;
            playerStatsHandler.runtimeStats.playerJump  = playerStats.jumpByLevel[statsP[2]];
            RecomputePoints();
            statBlockUI.UpdateUI();
        }
    }
}
