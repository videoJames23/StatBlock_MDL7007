using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class StatBlockUI : MonoBehaviour
{
    public TextMeshProUGUI[] valueTexts;

   
    public int[] statsP = {1, 1, 1};
    public int[] statsE = {1, 1, 1};
    private int selectedIndex;
    
    public PlayerController playerController;
    public EnemyController enemyController;
    public GameManager gameManagerScript;

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
    public int prevSize;


    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            enemyController = enemy.GetComponent<EnemyController>();
        }
        
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        holder = GameObject.FindGameObjectWithTag("Holder");
        background = GameObject.Find("Background");
        holderRT = holder.GetComponent<RectTransform>();
        
        
        GameObject size = GameObject.FindGameObjectWithTag("Size");
        GameObject jump = GameObject.FindGameObjectWithTag("Jump");
        showHideS = size.GetComponent<ShowHide>();
        showHideJ = jump.GetComponent<ShowHide>();

        selectedIndex = 0;

        holder.SetActive(false);
        // for (int i = 0; i < valueTexts.Length; i++)
        // {
        //     
        //     valueTexts[i].text = " ";
        //     showHideJ.Hide();
        //     showHideS.Hide();
        //     
        // }
        UIPosition = holderRT.anchoredPosition;
        



        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
        
    }

    void Update()
    {
        
        
           
        if (playerController == null)
        {
                
        }

        else
        {
            if (playerController.bInMenu)
            {


                // select stat
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    selectedIndex--;
                    if (selectedIndex < 0 && playerController.bInMenuP)
                    {
                        selectedIndex = statsP.Length - 1;
                    }
                    else if (selectedIndex < 0 && playerController.bInMenuE)
                    {
                        selectedIndex = statsE.Length - 1;
                    }

                    UpdateUI();
                }


                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    selectedIndex++;
                    if (selectedIndex >= statsP.Length && playerController.bInMenuP)
                    {
                        selectedIndex = 0;
                    }
                    else if (selectedIndex >= statsE.Length && playerController.bInMenuE)
                    {
                        selectedIndex = 0;
                    }

                    UpdateUI();
                }

                // change value
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    if (playerController.bInMenuP && iPointsLeftP > 0)
                    {
                        statsP[selectedIndex]++;

                        if (statsP[selectedIndex] > 3)
                        {
                            statsP[selectedIndex] = 3;
                        }
                        switch (selectedIndex)
                        {
                            case 0:
                                gameManagerScript.StatChangePHealth();
                                break;
                            case 1:
                                gameManagerScript.StatChangePSpeed();
                                break;
                            case 2:
                                gameManagerScript.StatChangePJump();
                                break;
                        }
                        
                        
                    }
                    else if (playerController.bInMenuE && iPointsLeftE > 0)
                    {
                        
                        if (selectedIndex == 2)
                        {
                            prevSize = statsE[2];
                        }
                        
                        statsE[selectedIndex]++;
                        
                        
                        
                        if (statsE[selectedIndex] > 3)
                        {
                            statsE[selectedIndex] = 3;
                        }

                        switch (selectedIndex)
                        {
                            case 0:
                            gameManagerScript.StatChangeEHealth();
                            break;
                            case 1:
                                gameManagerScript.StatChangeESpeed();
                                break;
                            case 2:
                                gameManagerScript.StatChangeESize();
                                break;
                        }
                        
                        
                    }

                    UpdateUI();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    if (playerController.bInMenuP)
                    {
                        statsP[selectedIndex]--;
                        
                        if (statsP[0] < 1)
                        {
                            statsP[0] = 1;
                        }

                        else if (statsP[1] < 0)
                        {
                            statsP[1] = 0;
                        }

                        else if (statsP[2] < 0)
                        {
                            statsP[2] = 0;
                        }
                        switch (selectedIndex)
                        {
                            case 0:
                                gameManagerScript.StatChangePHealth();
                                break;
                            case 1:
                                gameManagerScript.StatChangePSpeed();
                                break;
                            case 2:
                                gameManagerScript.StatChangePJump();
                                break;
                        }
                    }
                    else if (playerController.bInMenuE)
                    {
                        
                        if (selectedIndex == 2)
                        {
                            prevSize = statsE[2];
                        }
                        statsE[selectedIndex]--;

                        if (statsE[0] < 1)
                        {
                            statsE[0] = 1;
                        }

                        else if (statsE[1] < 0)
                        {
                            statsE[1] = 0;
                        }

                        else if (statsE[2] < 1)
                        {
                            statsE[2] = 1;
                        }
                        switch (selectedIndex)
                        {
                            case 0:
                                gameManagerScript.StatChangeEHealth();
                                break;
                            case 1:
                                gameManagerScript.StatChangeESpeed();
                                break;
                            case 2:
                                gameManagerScript.StatChangeESize();
                                break;
                        }
                        
                    }

                    UpdateUI();
                }



                
            }
        }
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
                    valueTexts[i].color = (i == selectedIndex) ? Color.green : Color.white;
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