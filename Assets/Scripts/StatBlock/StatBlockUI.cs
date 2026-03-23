using System;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private LevelConfigSO levelConfig;
    public bool IsReady { get; private set; } = false;
    
    private StatBlockInput statBlockInput;
    private StatBlockChangesP statBlockChangesP;
    private StatBlockChangesE  statBlockChangesE;
    
    public TextMeshProUGUI[] valueTexts;

    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    public PlayerController playerController;

    public GameObject holder;
    public GameObject background;
    [SerializeField] private RectTransform holderRT;
    [SerializeField] private Vector2 UIPosition;
    
    
    public ShowHide showHideJ;
    public ShowHide showHideS;
    
    public string sUser;
    
    
    public Vector3 vFocusScale = new Vector3(3f, 3f, 3f);
    public Vector2 vFocusPosition = new Vector2(-87.6f, -74.2f);
    public Vector3 vBackgroundFocusScale = new Vector3(10, 10, 10);
    public Vector3 vOutFocusScale = new Vector3(1f, 1f, 1f);
    public Vector3 vBackgroundOutFocusScale = new Vector3(2.82999992f,2.30865788f,1f);

    
    
    
    
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockChangesP = GetComponent<StatBlockChangesP>();
        statBlockChangesE = GetComponent<StatBlockChangesE>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        holder = GameObject.FindGameObjectWithTag("Holder");
        background = GameObject.Find("Background");
        holderRT = holder.GetComponent<RectTransform>();
        
        GameObject size = GameObject.FindGameObjectWithTag("Size");
        GameObject jump = GameObject.FindGameObjectWithTag("Jump");
        showHideS = size.GetComponent<ShowHide>();
        showHideJ = jump.GetComponent<ShowHide>();

        statBlockInput.selectedIndex = 0;

        holder.SetActive(false);
        
        IsReady = true;
        
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


    public void UpdateUI()
    {
        holder.SetActive(true);
        
        if (playerController.bInMenu)
        {
                holderRT.anchoredPosition = vFocusPosition;
                holderRT.localScale = vFocusScale;
                background.GetComponent<RectTransform>().localScale = vBackgroundFocusScale;

        }
        else if (!playerController.bInMenu)
        {
            holderRT.anchoredPosition = UIPosition;
            holderRT.localScale = vOutFocusScale;
            background.GetComponent<RectTransform>().localScale = vBackgroundOutFocusScale;
        }

        for (int i = 0; i < valueTexts.Length - 2; i++)
        {
            if (playerController)
            {
            
                if (playerController.bInMenuP)
                {
                    valueTexts[i].text = statBlockChangesP.statsP[i].ToString();
                    valueTexts[3].text = statBlockChangesP.iPointsLeftP.ToString();
                    showHideJ.Show();
                    showHideS.Hide();
                    sUser = "Player";
                    
                    
                }

                else if (playerController.bInMenuE)
                {
                    valueTexts[i].text = statBlockChangesE.statsE[i].ToString();
                    valueTexts[3].text = statBlockChangesE.iPointsLeftE.ToString();
                    showHideJ.Hide();
                    showHideS.Show();
                    sUser = "Enemy";

                }

                if (playerController.bInMenuP || playerController.bInMenuE)
                {
                    valueTexts[i].color = (i == statBlockInput.selectedIndex) ? Color.green : Color.white;
                }
                else if (!playerController.bInMenuP && !playerController.bInMenuE)
                {
                    valueTexts[i].color = Color.white;
                }
                valueTexts[4].text = sUser;
            }
        }
        
    }
}