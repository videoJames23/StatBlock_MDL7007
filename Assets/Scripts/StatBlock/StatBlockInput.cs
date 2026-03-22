using Unity.VisualScripting;
using UnityEngine;

public class StatBlockInput : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private EnemyStats enemyStats;
    
    private GameManager gameManagerScript;
    
    private StatBlockUI statBlockUI;
    private StatBlockChangesP statBlockChangesP;
    private StatBlockChangesE statBlockChangesE;
    
    [SerializeField] private PlayerStatsHandler playerStatsHandler;
    
    public delegate void Error();
    public static event Error OnError;

    public delegate void Up();
    public static event Up OnUp;
    public delegate void Down();
    public static event Down OnDown;
    public delegate void Index();
    public static event Index OnIndex;
    
    public int selectedIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statBlockUI = GetComponent<StatBlockUI>();
        statBlockChangesP = statBlockUI.GetComponent<StatBlockChangesP>();
        statBlockChangesE = statBlockUI.GetComponent<StatBlockChangesE>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
      
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
                    OnIndex?.Invoke();

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
                    OnIndex?.Invoke();
                    
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
                        if (statBlockChangesP.iPointsLeftP > 0)
                        {
                            statBlockChangesP.statsP[selectedIndex]++;
                            OnUp?.Invoke();
                            if (statBlockChangesP.statsP[selectedIndex] > playerStats.aPlayerStatBounds[selectedIndex, 1])
                            {
                                OnError?.Invoke();
                                statBlockChangesP.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 1];
                            }

                            switch (selectedIndex)
                            {
                                case 0:
                                    statBlockChangesP.StatChangePHealth();
                                    break;
                                case 1:
                                    statBlockChangesP.StatChangePSpeed();
                                    break;
                                case 2:
                                    statBlockChangesP.StatChangePJump();
                                    break;
                            }
                        }
                        else
                        {
                            OnError?.Invoke();
                        }


                    }
                    else if (playerController.bInMenuE)
                    {
                        if (statBlockChangesE.iPointsLeftE > 0)
                        {
                            
                            if (selectedIndex == 2)
                            {
                                statBlockUI.iPrevSize = statBlockChangesE.statsE[2];
                            }

                            statBlockChangesE.statsE[selectedIndex]++;
                            OnUp?.Invoke();



                            if (statBlockChangesE.statsE[selectedIndex] > enemyStats.aEnemyStatBounds[selectedIndex, 1])
                            {
                                OnError?.Invoke();
                                statBlockChangesE.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 1];
                            }

                            switch (selectedIndex)
                            {
                                case 0:
                                    statBlockChangesE.StatChangeEHealth();
                                    break;
                                case 1:
                                    statBlockChangesE.StatChangeESpeed();
                                    break;
                                case 2:
                                    statBlockChangesE.StatChangeESize();
                                    break;
                            }


                        }
                        else
                        {
                            OnError?.Invoke();
                        }

                        
                    }
                    statBlockUI.UpdateUI();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    OnDown?.Invoke();
                    if (playerController.bInMenuP)
                    {
                        statBlockChangesP.statsP[selectedIndex]--;
                        
                        if (statBlockChangesP.statsP[selectedIndex] < playerStats.aPlayerStatBounds[selectedIndex, 0])
                        {
                            OnError?.Invoke();
                            statBlockChangesP.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 0];
                        }
                        
                        switch (selectedIndex)
                        {
                            case 0:
                                statBlockChangesP.StatChangePHealth();
                                break;
                            case 1:
                                statBlockChangesP.StatChangePSpeed();
                                break;
                            case 2:
                                statBlockChangesP.StatChangePJump();
                                break;
                        }
                    }
                    else if (playerController.bInMenuE)
                    {
                        
                        if (selectedIndex == 2)
                        {
                            statBlockUI.iPrevSize = statBlockChangesE.statsE[2];
                        }

                        statBlockChangesE.statsE[selectedIndex]--;
                        
                        if (statBlockChangesE.statsE[selectedIndex] < enemyStats.aEnemyStatBounds[selectedIndex, 0])
                        {
                            OnError?.Invoke();
                            statBlockChangesE.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 0];
                        }
                        
                        switch (selectedIndex)
                        {
                            case 0:
                                statBlockChangesE.StatChangeEHealth();
                                break;
                            case 1:
                                statBlockChangesE.StatChangeESpeed();
                                break;
                            case 2:
                                statBlockChangesE.StatChangeESize();
                                break;
                        }
                        
                    }

                    statBlockUI.UpdateUI();
                }



                
            }
        }
    }
}
