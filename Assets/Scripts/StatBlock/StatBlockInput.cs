using Unity.VisualScripting;
using UnityEngine;

public class StatBlockInput : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private EnemyStats enemyStats;
    
    private GameManager gameManagerScript;
    
    private StatBlockUI statBlockUI;
    private StatBlockChanges statBlockChanges;
    
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
        statBlockChanges = statBlockUI.GetComponent<StatBlockChanges>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
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
                        if (statBlockChanges.iPointsLeftP > 0)
                        {
                            statBlockChanges.statsP[selectedIndex]++;
                            OnUp?.Invoke();
                            if (statBlockChanges.statsP[selectedIndex] > playerStats.aPlayerStatBounds[selectedIndex, 1])
                            {
                                OnError?.Invoke();
                                statBlockChanges.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 1];
                            }

                            switch (selectedIndex)
                            {
                                case 0:
                                    statBlockChanges.StatChangePHealth();
                                    break;
                                case 1:
                                    statBlockChanges.StatChangePSpeed();
                                    break;
                                case 2:
                                    statBlockChanges.StatChangePJump();
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
                        if (statBlockChanges.iPointsLeftE > 0)
                        {
                            
                            if (selectedIndex == 2)
                            {
                                statBlockUI.iPrevSize = statBlockChanges.statsE[2];
                            }

                            statBlockChanges.statsE[selectedIndex]++;
                            OnUp?.Invoke();



                            if (statBlockChanges.statsE[selectedIndex] > enemyStats.aEnemyStatBounds[selectedIndex, 1])
                            {
                                OnError?.Invoke();
                                statBlockChanges.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 1];
                            }

                            switch (selectedIndex)
                            {
                                case 0:
                                    statBlockChanges.StatChangeEHealth();
                                    break;
                                case 1:
                                    statBlockChanges.StatChangeESpeed();
                                    break;
                                case 2:
                                    statBlockChanges.StatChangeESize();
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
                        statBlockChanges.statsP[selectedIndex]--;
                        
                        if (statBlockChanges.statsP[selectedIndex] < playerStats.aPlayerStatBounds[selectedIndex, 0])
                        {
                            OnError?.Invoke();
                            statBlockChanges.statsP[selectedIndex] = playerStats.aPlayerStatBounds[selectedIndex, 0];
                        }
                        
                        switch (selectedIndex)
                        {
                            case 0:
                                statBlockChanges.StatChangePHealth();
                                break;
                            case 1:
                                statBlockChanges.StatChangePSpeed();
                                break;
                            case 2:
                                statBlockChanges.StatChangePJump();
                                break;
                        }
                    }
                    else if (playerController.bInMenuE)
                    {
                        
                        if (selectedIndex == 2)
                        {
                            statBlockUI.iPrevSize = statBlockChanges.statsE[2];
                        }

                        statBlockChanges.statsE[selectedIndex]--;
                        
                        if (statBlockChanges.statsE[selectedIndex] < enemyStats.aEnemyStatBounds[selectedIndex, 0])
                        {
                            OnError?.Invoke();
                            statBlockChanges.statsE[selectedIndex] = enemyStats.aEnemyStatBounds[selectedIndex, 0];
                        }
                        
                        switch (selectedIndex)
                        {
                            case 0:
                                statBlockChanges.StatChangeEHealth();
                                break;
                            case 1:
                                statBlockChanges.StatChangeESpeed();
                                break;
                            case 2:
                                statBlockChanges.StatChangeESize();
                                break;
                        }
                        
                    }

                    statBlockUI.UpdateUI();
                }



                
            }
        }
    }
}
