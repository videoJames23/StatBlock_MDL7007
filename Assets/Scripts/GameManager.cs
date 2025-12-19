using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private StatBlockUI statBlockUI;
    public int iBuildIndex;
    
    private AudioSource completionSource;
    private AudioSource jumpSource;
    private AudioSource openSource;
    private AudioSource closeSource;

    private Vector2 vPlayerVelocity;
    private Vector2 vEnemyVelocity;
    
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
        
        else if (!playerController.bInMenuP && !playerController.bInMenuE)
        {
            playerController.bInMenu = false;
        }
        statBlockUI.UpdateUI();
    }

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
                case 0: playerController.fPlayerSpeed = 0; break;
                case 1: playerController.fPlayerSpeed = 3; break;
                case 2: playerController.fPlayerSpeed = 7; break;
                case 3: playerController.fPlayerSpeed = 10; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statBlockUI.statsP[2]) //player jump heights
            {
                case 0: playerController.fPlayerJump = 0; break;
                case 1: playerController.fPlayerJump = 5; break;
                case 2: playerController.fPlayerJump = 7; break;
                case 3: playerController.fPlayerJump = 9; break;
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
                case 0: enemyController.fEnemySpeed = 0; break;
                case 1: enemyController.fEnemySpeed = 3; break;
                case 2: enemyController.fEnemySpeed = 7; break;
                case 3: enemyController.fEnemySpeed = 10; break;
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
                case 1:
                    enemyController.fEnemySize = 1.5f;
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
                            enemyController.fEnemySize = 3f;
                            break;
                        case 3:
                            enemyController.fEnemySize = 3f;
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.726443f);
                            break;
                    }

                    break;
                case 3:
                    if (statBlockUI.iPrevSize != 3)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.726443f);
                        enemyController.fEnemySize = 4.5f;
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
