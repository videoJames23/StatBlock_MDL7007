using UnityEngine;
using TMPro;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private LevelConfigSO levelConfig;
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    
    private StatBlockInput statBlockInput;
    private StatBlockChangesP statBlockChangesP;
    private StatBlockChangesE  statBlockChangesE;
    
    [SerializeField] private GameManager gameManager;
    
    private PlayerController playerController;
    
    [SerializeField] private TextMeshProUGUI[] valueTexts;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject background;
    [SerializeField] private RectTransform holderRT;
    [SerializeField] private Vector2 UIPosition;
    
    
    
    [SerializeField] private ShowHide showHideJump;
    [SerializeField] private ShowHide showHideSpeed;
    private string sUser;
    
    
    public Vector3 vFocusScale = new (3f, 3f, 3f);
    public Vector2 vFocusPosition = new (-87.6f, -74.2f);
    public Vector3 vBackgroundFocusScale = new (10, 10, 10);
    public Vector3 vOutFocusScale = new (1f, 1f, 1f);
    public Vector3 vBackgroundOutFocusScale = new (2.82999992f,2.30865788f,1f);
    
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

    // private MenuMode currentMode = MenuMode.None;
    
    
    [SerializeField] private MenuMode currentMode = MenuMode.None;

    public MenuMode CurrentMode
    {
        get => currentMode;
        private set => currentMode = value;
    }


    
    
    
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockChangesP = GetComponent<StatBlockChangesP>();
        statBlockChangesE = GetComponent<StatBlockChangesE>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        holderRT = holder.GetComponent<RectTransform>();

        statBlockInput.selectedIndex = 0;
        
        if (levelConfig != null)
            ApplyLevelUILayout(levelConfig);

        
    }
    
    
    public void ApplyLevelUILayout(LevelConfigSO config)
    {
        
        if (config == null) { Debug.LogWarning("[StatBlockUI] Config is null."); return; }
        if (!holderRT)      { Debug.LogWarning("[StatBlockUI] holderRT is null; Start() likely hasn’t run."); return; }

        Debug.Log($"[StatBlockUI] Applying UI: pos={config.uiHolderAnchoredPosition}, " + 
                  $"scale={config.uiHolderScale}, bgScale={config.uiBackgroundScale}");

        UIPosition = config.uiHolderAnchoredPosition;
        holderRT.anchoredPosition = UIPosition;
        holderRT.localScale = config.uiHolderScale;

        var bgRT = background?.GetComponent<RectTransform>();
        if (bgRT) bgRT.localScale = config.uiBackgroundScale;

        Debug.Log($"[StatBlockUI] After apply: holderRT.anchoredPosition={holderRT.anchoredPosition}, " +
                  $"localScale={holderRT.localScale}");
        
        holder.SetActive(false);

    }
    public void SetMenuMode(MenuMode mode)
    {
        currentMode = mode;
        UpdateUI();
    }

    
    public void TogglePlayerMenu()
    {
        if (currentMode == MenuMode.PlayerMenu)
        {
            SetMenuMode(MenuMode.None);
            OnMenuClose?.Invoke();
        }
        else if (!currentMode.Equals(MenuMode.PlayerMenu))
        {
            SetMenuMode(MenuMode.PlayerMenu);
            OnMenuOpen?.Invoke();
        }
    }

    
    public void ToggleEnemyMenu()
    {
        if (currentMode == MenuMode.EnemyMenu)
        {
            SetMenuMode(MenuMode.None);
            OnMenuClose?.Invoke();
        }
        else if (!currentMode.Equals(MenuMode.EnemyMenu))
        {
            SetMenuMode(MenuMode.EnemyMenu);
            OnMenuOpen?.Invoke();
        }
        
    }


    void DamageMenuRefreshP()
    {
        SetMenuMode(MenuMode.PlayerPreview);
    }
    
    void DamageMenuRefreshE()
    { 
        SetMenuMode(MenuMode.EnemyPreview);
    }

    public void UpdateUI()
    {
        
        
        if (CurrentMode == MenuMode.PlayerMenu || CurrentMode == MenuMode.EnemyMenu)
        {
            holder.SetActive(true);
            holderRT.anchoredPosition = vFocusPosition;
            holderRT.localScale = vFocusScale;
            background.GetComponent<RectTransform>().localScale = vBackgroundFocusScale;

        }
        else if (CurrentMode == MenuMode.PlayerPreview || CurrentMode == MenuMode.EnemyPreview)
        {
            holder.SetActive(true);
            holderRT.anchoredPosition = UIPosition;
            holderRT.localScale = vOutFocusScale;
            background.GetComponent<RectTransform>().localScale = vBackgroundOutFocusScale;
        }
        else if (CurrentMode == MenuMode.None)
        {
            holder.SetActive(false);
        }

        for (int i = 0; i < valueTexts.Length - 2; i++)
        {
            if (playerController)
            {
                if (CurrentMode == MenuMode.PlayerMenu ||
                    CurrentMode == MenuMode.PlayerPreview)
                {
                    valueTexts[i].text = statBlockChangesP.statsP[i].ToString();
                    valueTexts[3].text = statBlockChangesP.IPointsLeftP.ToString();
                    showHideJump.Show();
                    showHideSpeed.Hide();
                    sUser = "Player";
                }


                if (CurrentMode == MenuMode.EnemyMenu ||
                    CurrentMode == MenuMode.EnemyPreview)
                {
                    valueTexts[i].text = statBlockChangesE.statsE[i].ToString();
                    valueTexts[3].text = statBlockChangesE.IPointsLeftE.ToString();
                    showHideJump.Hide();
                    showHideSpeed.Show();
                    sUser = "Enemy";

                }
                
                bool isSelectionMode = CurrentMode == MenuMode.PlayerMenu || CurrentMode == MenuMode.EnemyMenu;
                {
                    valueTexts[i].color =
                        isSelectionMode && i == statBlockInput.selectedIndex
                            ? Color.green
                            : Color.white;

                }
                valueTexts[4].text = sUser;
            }
        }
        
    }
}