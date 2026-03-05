using System;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StatBlockUI : MonoBehaviour
{
    private StatBlockInput statBlockInput;
    
    public TextMeshProUGUI[] valueTexts;

    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    public int[] statsP = {1, 1, 1};
    public int[] statsE = {1, 1, 1};
    
    public PlayerController playerController;
    public GameManager gameManagerScript;
    public AudioController audioController;

    public GameObject holder;
    public GameObject background;
    private RectTransform holderRT;
    private Vector2 UIPosition;
    
    
    public ShowHide showHideJ;
    public ShowHide showHideS;
    
 
    public int iPointsTotalP;
    public int iPointsLeftP;
    public int iPointsTotalE;
    public int iPointsLeftE;
    public string sUser;
    [FormerlySerializedAs("prevSize")] public int iPrevSize;

    
    
    
    
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        holder = GameObject.FindGameObjectWithTag("Holder");
        background = GameObject.Find("Background");
        holderRT = holder.GetComponent<RectTransform>();
        
        
        GameObject size = GameObject.FindGameObjectWithTag("Size");
        GameObject jump = GameObject.FindGameObjectWithTag("Jump");
        showHideS = size.GetComponent<ShowHide>();
        showHideJ = jump.GetComponent<ShowHide>();
        
        GameObject audio = GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController = audio.GetComponent<AudioController>();
        }
        
        
        
        
        

        statBlockInput.selectedIndex = 0;

        holder.SetActive(false);
        
        UIPosition = holderRT.anchoredPosition;
        
        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
        
    }

    void Update()
    {
        
       
    }

    public void UpdateUI()
    {
        holder.SetActive(true);
        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
        
        
        if (playerController.bInMenu)
        {
                holderRT.anchoredPosition = new Vector2(-87.6f, -74.2f);
                holderRT.localScale = new Vector3(3f, 3f, 3f);
                background.GetComponent<RectTransform>().localScale = new Vector3(10, 10, 10);
            
        }
        else if (!playerController.bInMenu)
        {
            holderRT.anchoredPosition = UIPosition;
            holderRT.localScale = new Vector3(1f, 1f, 1f);
            background.GetComponent<RectTransform>().localScale = new Vector3(2.82999992f,2.30865788f,1f);
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
                    valueTexts[i].text = statsP[i].ToString();
                    valueTexts[3].text = iPointsLeftP.ToString();
                    showHideJ.Show();
                    showHideS.Hide();
                    sUser = "Player";
                    
                    
                }

                else if (playerController.bInMenuE)
                {
                    valueTexts[i].text = statsE[i].ToString();
                    valueTexts[3].text = iPointsLeftE.ToString();
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