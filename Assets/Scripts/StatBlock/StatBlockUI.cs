using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private LevelConfigSO levelConfig;
    
    private StatBlockInput statBlockInput;
    private StatBlockChangesP statBlockChangesP;
    private StatBlockChangesE  statBlockChangesE;
    
    [SerializeField] private PlayerController playerController;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI[] valueTexts;
    [SerializeField] private GameObject holder;
    [SerializeField] private RectTransform holderRT;
    [SerializeField] private GameObject background;
    [SerializeField] private RectTransform backgroundRT;
    [SerializeField] private Vector2 uiPosition;
    [SerializeField] private ShowHide showHideJump;
    [SerializeField] private ShowHide showHideSpeed;
    
    [Header ("UI Positions")]
    [SerializeField] private Vector3 vFocusScale = new (3f, 3f, 3f);
    [SerializeField] private Vector2 vFocusPosition = new (-87.6f, -74.2f);
    [SerializeField] private Vector3 vBackgroundFocusScale = new (10, 10, 10);
    [SerializeField] private Vector3 vOutFocusScale = new (1f, 1f, 1f);
    [SerializeField] private Vector3 vBackgroundOutFocusScale = new (2.82999992f,2.30865788f,1f);
    
    private string sUser;
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

        var wasOpen = currentMode == MenuMode.PlayerMenu || currentMode == MenuMode.EnemyMenu;

        currentMode = mode;
        UpdateUI();

        var isOpen = currentMode == MenuMode.PlayerMenu || currentMode == MenuMode.EnemyMenu;

        if (!wasOpen && isOpen)
            OnMenuOpen?.Invoke();
        else if (wasOpen && !isOpen)
            OnMenuClose?.Invoke();

    }
    
    private void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockChangesP = GetComponent<StatBlockChangesP>();
        statBlockChangesE = GetComponent<StatBlockChangesE>();
        
        holderRT = holder.GetComponent<RectTransform>();
        
        if (levelConfig != null)
            ApplyLevelUILayout(levelConfig);
    }
    
    
    public void ApplyLevelUILayout(LevelConfigSO config)
    {

        if (config == null)
        {
            Debug.LogWarning("[StatBlockUI] Config is null."); return;
        }

        if (!holderRT)
        {
            Debug.LogWarning("[StatBlockUI] holderRT is null; Start() likely hasn’t run."); return;
        }

        Debug.Log($"[StatBlockUI] Applying UI: pos={config.uiHolderAnchoredPosition}, " + 
                  $"scale={config.uiHolderScale}, bgScale={config.uiBackgroundScale}");

        uiPosition = config.uiHolderAnchoredPosition;
        holderRT.anchoredPosition = uiPosition;
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
            SetMenuMode(MenuMode.EnemyPreview);
        }
        else if (!currentMode.Equals(MenuMode.EnemyMenu))
        {
            SetMenuMode(MenuMode.EnemyMenu);
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
        if (!holder || !holderRT || !background)
        {
            return;
        }
        
        switch (CurrentMode)
        {
            case MenuMode.PlayerMenu:
            case MenuMode.EnemyMenu:
                holder.SetActive(true);
                holderRT.anchoredPosition = vFocusPosition;
                holderRT.localScale = vFocusScale;
                backgroundRT.localScale = vBackgroundFocusScale;
                break;
            case MenuMode.PlayerPreview:
            case MenuMode.EnemyPreview:
                holder.SetActive(true);
                holderRT.anchoredPosition = uiPosition;
                holderRT.localScale = vOutFocusScale;
                backgroundRT.localScale = vBackgroundOutFocusScale;
                break;
            case MenuMode.None:
                holder.SetActive(false);
                break;
        }

        for (int i = 0; i < statCount; i++)
        {
            if (!playerController) continue;
            if (CurrentMode == MenuMode.PlayerMenu || CurrentMode == MenuMode.PlayerPreview)
            {
                valueTexts[i].text = statBlockChangesP.statsP[i].ToString();
                valueTexts[3].text = statBlockChangesP.IPointsLeftP.ToString();
                showHideJump.Show();
                showHideSpeed.Hide();
                sUser = "Player";
            }


            if (CurrentMode == MenuMode.EnemyMenu || CurrentMode == MenuMode.EnemyPreview)
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
                    isSelectionMode && i == statBlockInput.SelectedIndex
                        ? Color.green
                        : Color.white;

            }
            valueTexts[4].text = sUser;
        }
        
    }
}