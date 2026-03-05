using UnityEngine;

public class StatBlockInput : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private EnemyStats enemyStats;
    
    private GameManager gameManagerScript;
    
    private AudioController audioController;
    
    private StatBlockUI statBlockUI;
    
    public int selectedIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statBlockUI = GetComponent<StatBlockUI>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject audio =  GameObject.FindGameObjectWithTag("Audio");
        audioController = audio.GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
         if (playerController)
        {
            if (playerController.bInMenu)
            {
            

                // select stat
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    selectedIndex--;
                    audioController.indexSource.Play();

                    switch (selectedIndex)
                    {
                        case -1: selectedIndex = 2; break;
                        case 0: break;
                        case 1: break;
                        case 2: break;
                    }
                    statBlockUI.UpdateUI();
                }


                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    selectedIndex++;
                    audioController.indexSource.Play();
                    
                    switch (selectedIndex)
                    {
                        case 0: break;
                        case 1: break;
                        case 2: break;
                        case 3 : selectedIndex = 0; break;
                    }

                    statBlockUI.UpdateUI();
                }

                // change value
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    
                    if (playerController.bInMenuP)
                    {
                        if (statBlockUI.iPointsLeftP > 0)
                        {
                            statBlockUI.statsP[selectedIndex]++;
                            audioController.upSource.Play();
                            if (statBlockUI.statsP[selectedIndex] > playerStats.aPlayerStatBounds[selectedIndex, 1])
                            {
                                audioController.errorSource.Play();
                                statBlockUI.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 1];
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
                        else
                        {
                            audioController.errorSource.Play();
                        }


                    }
                    else if (playerController.bInMenuE)
                    {
                        if (statBlockUI.iPointsLeftE > 0)
                        {
                            
                            if (selectedIndex == 2)
                            {
                                statBlockUI.iPrevSize = statBlockUI.statsE[2];
                            }

                            statBlockUI.statsE[selectedIndex]++;
                            audioController.upSource.Play();



                            if (statBlockUI.statsE[selectedIndex] > enemyStats.aEnemyStatBounds[selectedIndex, 1])
                            {
                                audioController.errorSource.Play();
                                statBlockUI.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 1];
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
                        else
                        {
                            audioController.errorSource.Play();
                        }

                        
                    }
                    statBlockUI.UpdateUI();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    audioController.downSource.Play();
                    if (playerController.bInMenuP)
                    {
                        statBlockUI.statsP[selectedIndex]--;
                        
                        if (statBlockUI.statsP[selectedIndex] < playerStats.aPlayerStatBounds[selectedIndex, 0])
                        {
                            audioController.errorSource.Play();
                            statBlockUI.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 0];
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
                            statBlockUI.iPrevSize = statBlockUI.statsE[2];
                        }

                        statBlockUI.statsE[selectedIndex]--;
                        
                        if (statBlockUI.statsE[selectedIndex] < enemyStats.aEnemyStatBounds[selectedIndex, 0])
                        {
                            audioController.errorSource.Play();
                            statBlockUI.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 0];
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

                    statBlockUI.UpdateUI();
                }



                
            }
        }
    }
}
