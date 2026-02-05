using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    //serializedfield stuff here again -F
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private StatBlockUI statBlockUI;
    public int iBuildIndex;
    
    //audio stuff could have gone into music.cs maybe? idk if you were planning on that -F
    private AudioSource completionSource;
    private AudioSource jumpSource;
    private AudioSource openSource;
    private AudioSource closeSource;

    private Vector2 vPlayerVelocity;
    private Vector2 vEnemyVelocity;

    //Removing magic numbers using predetermined values for each stat level -F
    [SerializeField] private float playerSpeedLVL0 = 0f;
    [SerializeField] private float playerSpeedLVL1 = 3f;
    [SerializeField] private float playerSpeedLVL2 = 7f;
    [SerializeField] private float playerSpeedLVL3 = 10f;
    
    [SerializeField] private float playerJumpLVL0 = 0f;
    [SerializeField] private float playerJumpLVL1 = 5f;
    [SerializeField] private float playerJumpLVL2 = 7f;
    [SerializeField] private float playerJumpLVL3 = 9f;

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
        
        
        GameObject openAudio = GameObject.Find("Open");
        if (openAudio)
        {
            openSource = openAudio.GetComponent<AudioSource>();
        }
        
        GameObject closeAudio = GameObject.Find("Close");
        if (closeAudio)
        {
            closeSource = closeAudio.GetComponent<AudioSource>();
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
                openSource.Play();
            }
            else if (!playerController.bInMenuP)
            {
                closeSource.Play();
            }
            MenuChecks();
            MenuFreezeToggle();
        }
        
        if (playerController.bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E))
        {
            if (!playerController.bInMenuE)
            {
                playerController.bInMenuE = true;
                openSource.Play();
                enemyController.fPrevDir = enemyController.fEnemyDir;
                MenuChecks();
            }
            else if (playerController.bInMenuE)
            {
                if (statBlockUI.iPointsLeftE == 0)
                {
                    playerController.bInMenuE = false;
                    closeSource.Play();
                    enemyController.fEnemyDir = enemyController.fPrevDir;
                    MenuChecks();
                }
                else if (statBlockUI.iPointsLeftE > 0)
                {
                    statBlockUI.errorSource.Play(); 
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
        
        else //if (!playerController.bInMenuP && !playerController.bInMenuE) redundant condition -F
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
                vPlayerVelocity = playerRb.linearVelocity;
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
                playerRb.linearVelocity =  vPlayerVelocity;
            }
                
        }
    }
    
    public void StatChangePHealth()
    {
        playerController.iPlayerHealth = statBlockUI.statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statBlockUI.statsP[1]) // player speeds
            {
                //MAGIC NUMBERS RAAAAAAAAAAAAAAAAH I HATE MAGIC NUMBERS -F
                case 0: playerController.fPlayerSpeed = playerSpeedLVL0; break;
                case 1: playerController.fPlayerSpeed = playerSpeedLVL1; break;
                case 2: playerController.fPlayerSpeed = playerSpeedLVL2; break;
                case 3: playerController.fPlayerSpeed = playerSpeedLVL3; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statBlockUI.statsP[2]) //player jump heights
            {
                //MAGIC NUMBERS RAAAAAAAAAAAAAAAAH I HATE MAGIC NUMBERS -F
                case 0: playerController.fPlayerJump = playerJumpLVL0; break;
                case 1: playerController.fPlayerJump = playerJumpLVL1; break;
                case 2: playerController.fPlayerJump = playerJumpLVL2; break;
                case 3: playerController.fPlayerJump = playerJumpLVL3; break;
            }
        }
    }
    
    
    public void StatChangeEHealth()
    {
        if (enemyController)
        {
            enemyController.iEnemyHealth = statBlockUI.statsE[0];
        }
    }
    public void StatChangeESpeed()
    {
        if (enemyController)
        {
            switch (statBlockUI.statsE[1]) // enemy speeds
            {
                //MAGIC NUMBERS RAAAAAAAAAAAAAAAAH I HATE MAGIC NUMBERS -F
                case 0: enemyController.fEnemySpeed = enemySpeedLVL0; break;
                case 1: enemyController.fEnemySpeed = enemySpeedLVL1; break;
                case 2: enemyController.fEnemySpeed = enemySpeedLVL2; break;
                case 3: enemyController.fEnemySpeed = enemySpeedLVL3; break;
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
                    enemyController.fEnemySize = enemySizeLVL1;
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
                            enemyController.fEnemySize = enemySizeLVL2;
                            break;
                        case 3:
                            enemyController.fEnemySize = enemySizeLVL2;
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.726443f);
                            break;
                    }

                    break;
                case 3:
                    if (statBlockUI.iPrevSize != 3)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.726443f);
                        enemyController.fEnemySize = enemySizeLVL3;
                    }

                    break;
            }
            enemyRb.transform.localScale = new Vector2(enemyController.fEnemySize, enemyController.fEnemySize);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
