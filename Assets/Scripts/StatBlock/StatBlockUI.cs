using Player;
using Scriptable_Objects.LevelConfigs;
using TMPro;
using UnityEngine;

namespace StatBlock
{
    public class StatBlockUI : MonoBehaviour
    {
        // Responsible for:
        // tracking and exposing UI menu mode
        // &
        // updating UI to reflect stat changes
        
        [Header ("LevelConfig")]
        [SerializeField] private LevelConfigSO levelConfig;
        [SerializeField] private LevelBootstrap levelBootstrap;
    
        [Header ("StatBlock")]
        [SerializeField] private StatBlockInput statBlockInput;
        [SerializeField] private StatBlockChangesP statBlockChangesP;
        [SerializeField] private StatBlockChangesE  statBlockChangesE;
    
        [SerializeField] private PlayerController playerController;
    
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI[] valueTexts;
        [SerializeField] private GameObject holder;
        [SerializeField] private RectTransform holderRT;
        [SerializeField] private GameObject background;
        [SerializeField] private RectTransform backgroundRT;
    
        [SerializeField] private ShowHide showHideJump;
        [SerializeField] private ShowHide showHideSpeed;
    
        [Header ("UI Positions")]
        [SerializeField] private Vector3 focusScale = new (3f, 3f, 3f);
        [SerializeField] private Vector2 focusPosition = new (-87.6f, -74.2f);
        [SerializeField] private Vector3 backgroundFocusScale = new (10, 10, 10);
        [SerializeField] private Vector3 outFocusScale = new (1f, 1f, 1f);
        [SerializeField] private Vector2 outFocusPosition;
        [SerializeField] private Vector3 backgroundOutFocusScale = new (2.82999992f,2.30865788f,1f);
    
        private string user;
        private const int statCount = 3;
    
        public delegate void MenuOpen();
        public static event MenuOpen OnMenuOpen;

        public delegate void MenuClose();
        public static event MenuClose OnMenuClose;


        private void OnEnable()
        {
            StatBlockChangesP.OnDamageRefresh += DamageMenuRefreshP;
            StatBlockChangesE.OnDamageRefresh += DamageMenuRefreshE;
        }

        private void OnDisable()
        {
            StatBlockChangesP.OnDamageRefresh -= DamageMenuRefreshP;
            StatBlockChangesE.OnDamageRefresh -= DamageMenuRefreshE;
        }
    
        public enum MenuMode
        {
            None,
            PlayerMenu,
            EnemyMenu,
            PlayerPreview,
            EnemyPreview
        }
    
        [SerializeField] private MenuMode currentMode = MenuMode.None;

        public MenuMode CurrentMode
        {
            get => currentMode;
            private set => currentMode = value;
        }

        public void SetMenuMode(MenuMode mode)
        {
            if (CurrentMode == mode)
                return;

            var wasOpen = CurrentMode is MenuMode.PlayerMenu or MenuMode.EnemyMenu;

            CurrentMode = mode;
            UpdateUI();

            var isOpen = CurrentMode is MenuMode.PlayerMenu or MenuMode.EnemyMenu;

            switch (wasOpen)
            {
                case false when isOpen:
                    OnMenuOpen?.Invoke();
                    break;
                case true when !isOpen:
                    OnMenuClose?.Invoke();
                    break;
            }

        }
    
        private void Start()
        {
            ApplyLevelUILayout(levelConfig);
        }
    
    
        public void ApplyLevelUILayout(LevelConfigSO config)
        {

            if (!config)
            {
                Debug.LogWarning("[StatBlockUI] Config is null."); return;
            }

            if (!holderRT)
            {
                Debug.LogWarning("[StatBlockUI] holderRT is null; Start() likely hasn’t run."); return;
            }

            Debug.Log($"[StatBlockUI] Applying UI: pos={config.uiHolderAnchoredPosition}, " + 
                      $"scale={config.uiHolderScale}, bgScale={config.uiBackgroundScale}");

            outFocusPosition = config.uiHolderAnchoredPosition;
            holderRT.anchoredPosition = outFocusPosition;
            holderRT.localScale = config.uiHolderScale;
        
            if (backgroundRT)
            {
                backgroundRT.localScale = config.uiBackgroundScale;
            }

            Debug.Log($"[StatBlockUI] After apply: holderRT.anchoredPosition={holderRT.anchoredPosition}, " +
                      $"localScale={holderRT.localScale}");
        
            holder.SetActive(false);

        }
    

    
        public void TogglePlayerMenu()
        {
            if (currentMode == MenuMode.PlayerMenu)
            {
                // Closing menu returns to preview in order to show current stats
                SetMenuMode(MenuMode.PlayerPreview);
            }
            else if (!currentMode.Equals(MenuMode.PlayerMenu))
            {
                SetMenuMode(MenuMode.PlayerMenu);
            }
        }

    
        public void ToggleEnemyMenu()
        {
            if (currentMode == MenuMode.EnemyMenu)
            {
                // Closing menu returns to preview in order to show current stats
                SetMenuMode(MenuMode.EnemyPreview);
            }
            else if (!currentMode.Equals(MenuMode.EnemyMenu))
            {
                SetMenuMode(MenuMode.EnemyMenu);
            }
        }

    
        // Damage updates preview to show current stats
        private void DamageMenuRefreshP()
        {
            SetMenuMode(MenuMode.PlayerPreview);
        }

        private void DamageMenuRefreshE()
        { 
            SetMenuMode(MenuMode.EnemyPreview);
        }

        public void UpdateUI()
        {
            if (!holder || !holderRT || !background)
            {
                return;
            }
        
            switch (CurrentMode)
            {
                case MenuMode.PlayerMenu:
                case MenuMode.EnemyMenu:
                    holder.SetActive(true);
                    holderRT.anchoredPosition = focusPosition;
                    holderRT.localScale = focusScale;
                    backgroundRT.localScale = backgroundFocusScale;
                    break;
                case MenuMode.PlayerPreview:
                case MenuMode.EnemyPreview:
                    holder.SetActive(true);
                    holderRT.anchoredPosition = outFocusPosition;
                    holderRT.localScale = outFocusScale;
                    backgroundRT.localScale = backgroundOutFocusScale;
                    break;
                case MenuMode.None:
                    holder.SetActive(false);
                    break;
            }

            for (int i = 0; i < statCount; i++)
            {
                if (!playerController) continue;
                if (CurrentMode is MenuMode.PlayerMenu or MenuMode.PlayerPreview)
                {
                    valueTexts[i].text = statBlockChangesP.statsP[i].ToString();
                    valueTexts[3].text = statBlockChangesP.PointsLeftP.ToString();
                    showHideJump.Show();
                    showHideSpeed.Hide();
                    user = "Player";
                }
            
                if (CurrentMode is MenuMode.EnemyMenu or MenuMode.EnemyPreview)
                {
                    valueTexts[i].text = statBlockChangesE.statsE[i].ToString();
                    valueTexts[3].text = statBlockChangesE.PointsLeftE.ToString();
                    showHideJump.Hide();
                    showHideSpeed.Show();
                    user = "Enemy";

                }
                
                var isSelectionMode = CurrentMode is MenuMode.PlayerMenu or MenuMode.EnemyMenu;
                {
                    valueTexts[i].color =
                        isSelectionMode && i == statBlockInput.SelectedIndex
                            ? Color.green
                            : Color.white;
                }
                valueTexts[4].text = user;
            }
        }
    }
}