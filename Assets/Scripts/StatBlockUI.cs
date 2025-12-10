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
    public ShowHide showHideJ;
    public ShowHide showHideS;
    
 
    public int iPointsTotalP;
    public int iPointsLeftP;
    public int iPointsTotalE;
    public int iPointsLeftE;
    public string sUser;


    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyController = enemy.GetComponent<EnemyController>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject size = GameObject.FindGameObjectWithTag("Size");
        GameObject jump = GameObject.FindGameObjectWithTag("Jump");
        showHideS = size.GetComponent<ShowHide>();
        showHideJ = jump.GetComponent<ShowHide>();


        for (int i = 0; i < valueTexts.Length; i++)
        {
            valueTexts[i].text = " ";
            showHideJ.Hide();
            showHideS.Hide();
            
        }




        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsLeftP - statsE.Sum();
        UpdateUI();
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
                        gameManagerScript.StatChangeP();
                        
                        
                    }
                    else if (playerController.bInMenuE && iPointsLeftE > 0)
                    {
                        statsE[selectedIndex]++;

                        if (statsE[selectedIndex] > 3)
                        {
                            statsE[selectedIndex] = 3;
                        }
                        gameManagerScript.StatChangeE();
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
                        gameManagerScript.StatChangeP();
                    }
                    else if (playerController.bInMenuE)
                    {
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
                        gameManagerScript.StatChangeE();
                    }

                    UpdateUI();
                }



                UpdateUI();
            }
        }
    }

    public void UpdateUI()
    {
        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
        
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
                    
                }

                else if (playerController.bInMenuE)
                {
                    valueTexts[i].text = statsE[i].ToString();
                    valueTexts[3].text = iPointsLeftE.ToString();
                    showHideJ.Hide();
                    showHideS.Show();

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