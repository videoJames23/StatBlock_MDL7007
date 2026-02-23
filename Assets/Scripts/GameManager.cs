using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //serializedfield stuff here again -F
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private StatBlockUI statBlockUI;
    private AudioController audioController;
    public int iBuildIndex;
    
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats enemyStats;
    
    private AudioSource completionSource;
    private AudioSource jumpSource;
    private AudioSource openSource;
    private AudioSource closeSource;

    
    private Vector2 vEnemyVelocity;

    
   

    [SerializeField] private float enemySpeedLVL0 = 0f;
    [SerializeField] private float enemySpeedLVL1 = 3f;
    [SerializeField] private float enemySpeedLVL2 = 7f;
    [SerializeField] private float enemySpeedLVL3 = 10f;
    
    [SerializeField] private float enemySizeLVL1 = 1.5f;
    [SerializeField] private float enemySizeLVL2 = 3f;
    [SerializeField] private float enemySizeLVL3 = 4.5f;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        iBuildIndex = SceneManager.GetActiveScene().buildIndex;
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy)
        {
            enemyRb = enemy.GetComponent<Rigidbody2D>();
            enemyController = enemy.GetComponent<EnemyController>();
        }
        GameObject audio =  GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController =  audio.GetComponent<AudioController>();
        }
        
       
        

        StatChangePHealth();
        StatChangePSpeed();
        StatChangePJump();
        StatChangeEHealth();
        StatChangeESpeed();
        StatChangeESize();
    }

    // Update is called once per frame
    void Update()
    {
        //there has to be a better way to do this that doesn't involve this many if/else statements -F
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(iBuildIndex);
        }
        
        if (playerController.bIsTouchingStatBlockP && Input.GetKeyDown(KeyCode.E))
        {
            playerController.bInMenuP = !playerController.bInMenuP;
            
            if (playerController.bInMenuP)
            {
                audioController.openSource.Play();
            }
            else if (!playerController.bInMenuP)
            {
                audioController.closeSource.Play();
            }
            MenuChecks();
            MenuFreezeToggle();
        }
        
        if (playerController.bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E))
        {
            if (!playerController.bInMenuE)
            {
                playerController.bInMenuE = true;
                audioController.openSource.Play();
                enemyController.fPrevDir = enemyController.fEnemyDir;
                MenuChecks();
            }
            else if (playerController.bInMenuE)
            {
                if (statBlockUI.iPointsLeftE == 0)
                {
                    playerController.bInMenuE = false;
                    audioController.closeSource.Play();
                    enemyController.fEnemyDir = enemyController.fPrevDir;
                    MenuChecks();
                }
                else if (statBlockUI.iPointsLeftE > 0)
                {

                }
            }
            MenuFreezeToggle();
        }
    }

    void MenuChecks()
    {
        if (playerController.bInMenuP || playerController.bInMenuE)
        {
            playerController.bInMenu = true;
        }
        
        else
        {
            playerController.bInMenu = false;
        }
        statBlockUI.UpdateUI();
    }

    //Pausing in general could be implemented better, getting rid of all of these if/else statements -F
    void MenuFreezeToggle()
    {
        if (playerController.bInMenu)
        {
            if (enemyRb)
            {
                vEnemyVelocity = enemyRb.linearVelocity;
                enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (playerRb)
            {
                playerStats.vPlayerVelocity = playerRb.linearVelocity;
                playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

        }
        else if (!playerController.bInMenu)
        {
            if (enemyRb)
            {
                enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                enemyRb.linearVelocity = vEnemyVelocity;
            }

            if (playerRb)
            {
                playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                playerRb.linearVelocity =  playerStats.vPlayerVelocity;
            }
                
        }
    }
    
    public void StatChangePHealth()
    {
        playerStats.iPlayerHealth = statBlockUI.statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statBlockUI.statsP[1]) // player speeds
            {
                
                case 0: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL0; break;
                case 1: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL1; break;
                case 2: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL2; break;
                case 3: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL3; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statBlockUI.statsP[2]) //player jump heights
            {
                
                case 0: playerStats.fPlayerJump = playerStats.playerJumpLVL0; break;
                case 1: playerStats.fPlayerJump = playerStats.playerJumpLVL1; break;
                case 2: playerStats.fPlayerJump = playerStats.playerJumpLVL2; break;
                case 3: playerStats.fPlayerJump = playerStats.playerJumpLVL3; break;
            }
        }
    }
    
    
    public void StatChangeEHealth()
    {
        if (enemyController)
        {
            enemyStats.iEnemyHealth = statBlockUI.statsE[0];
        }
    }
    public void StatChangeESpeed()
    {
        if (enemyController)
        {
            switch (statBlockUI.statsE[1]) // enemy speeds
            {
                case 0: enemyStats.fEnemySpeed = enemySpeedLVL0; break;
                case 1: enemyStats.fEnemySpeed = enemySpeedLVL1; break;
                case 2: enemyStats.fEnemySpeed = enemySpeedLVL2; break;
                case 3: enemyStats.fEnemySpeed = enemySpeedLVL3; break;
            }
        }
    }
    public void StatChangeESize()
    {
        if (enemyController)
        {
            switch (statBlockUI.statsE[2]) //enemy sizes
            {

                // If enemy grows into wall, movement stops
                
                //I'm assuming it's dependent on the differences between sizes, but the Y offset floats are more magic numbers,
                //which could be replaced with a calculation which works regardless of what you set the sizes to -F

                case 1:
                    enemyStats.fEnemySize = enemySizeLVL1;
                    if (statBlockUI.iPrevSize != 1)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.81f);
                    }

                    break;
                case 2:
                    
                    switch (statBlockUI.iPrevSize)
                    {
                        case 1:
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.81f);
                            enemyStats.fEnemySize = enemySizeLVL2;
                            break;
                        case 3:
                            enemyStats.fEnemySize = enemySizeLVL2;
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.726443f);
                            break;
                    }

                    break;
                case 3:
                    if (statBlockUI.iPrevSize != 3)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.726443f);
                        enemyStats.fEnemySize = enemySizeLVL3;
                    }

                    break;
            }
            enemyRb.transform.localScale = new Vector2(enemyStats.fEnemySize, enemyStats.fEnemySize);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
