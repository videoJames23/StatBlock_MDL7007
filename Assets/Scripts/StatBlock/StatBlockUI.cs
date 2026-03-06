using System;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StatBlockUI : MonoBehaviour
{
    private StatBlockInput statBlockInput;
    private StatBlockChanges statBlockChanges;
    
    public TextMeshProUGUI[] valueTexts;

    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    public PlayerController playerController;

    public GameObject holder;
    public GameObject background;
    private RectTransform holderRT;
    private Vector2 UIPosition;
    
    
    public ShowHide showHideJ;
    public ShowHide showHideS;
    
    public string sUser;
    [FormerlySerializedAs("prevSize")] public int iPrevSize;
    
    public Vector3 vFocusScale = new Vector3(3f, 3f, 3f);
    public Vector2 vFocusPosition = new Vector2(-87.6f, -74.2f);
    public Vector3 vBackgroundFocusScale = new Vector3(10, 10, 10);
    public Vector3 vOutFocusScale = new Vector3(1f, 1f, 1f);
    public Vector3 vBackgroundOutFocusScale = new Vector3(2.82999992f,2.30865788f,1f);

    
    
    
    
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockChanges = GetComponent<StatBlockChanges>();
        
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
        
        UIPosition = holderRT.anchoredPosition;
    }

    void Update()
    {
        
       
    }

    public void UpdateUI()
    {
        holder.SetActive(true);
        statBlockChanges.iPointsLeftP = statBlockChanges.iPointsTotalP - statBlockChanges.statsP.Sum();
        statBlockChanges.iPointsLeftE = statBlockChanges.iPointsTotalE - statBlockChanges.statsE.Sum();
        
        
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
            if (playerController == null)
            {
                
            }

            else
            {
                if (playerController.bInMenuP)
                {
                    valueTexts[i].text = statBlockChanges.statsP[i].ToString();
                    valueTexts[3].text = statBlockChanges.iPointsLeftP.ToString();
                    showHideJ.Show();
                    showHideS.Hide();
                    sUser = "Player";
                    
                    
                }

                else if (playerController.bInMenuE)
                {
                    valueTexts[i].text = statBlockChanges.statsE[i].ToString();
                    valueTexts[3].text = statBlockChanges.iPointsLeftE.ToString();
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