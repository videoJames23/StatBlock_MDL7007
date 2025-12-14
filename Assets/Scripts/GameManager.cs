using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private StatBlockUI statBlockUI;
    public int buildIndex;
    
   
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            enemyRb = enemy.GetComponent<Rigidbody2D>();
            enemyController = enemy.GetComponent<EnemyController>();
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
            SceneManager.LoadScene(buildIndex);
        }
        
        if (playerController != null)
        {
            if (playerController.bInMenu)
            {
                if (enemyRb)
                {
                    enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
                }

                if (playerRb)
                {
                    playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
                }

            }
            else if (!playerController.bInMenu)
            {
                if (enemyRb)
                {
                     enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                if (playerRb)
                {
                    playerRb.constraints = RigidbodyConstraints2D.None;
                }
                
            }
        }
        

        
    }

  
   

    public void StatChangePHealth()
    {
        playerController.iPlayerHealth = statBlockUI.statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController != null)
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
        if (playerController != null)
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
        if (enemyController != null)
        {
            enemyController.iEnemyHealth = statBlockUI.statsE[0];
        }
    }
    public void StatChangeESpeed()
    {
        if (enemyController != null)
        {
            switch (statBlockUI.statsE[1]) // enemy speeds
            {
                case 0: enemyController.fEnemySpeed = 0; break;
                case 1: enemyController.fEnemySpeed = 3 * enemyController.fEnemyDir; break;
                case 2: enemyController.fEnemySpeed = 7 * enemyController.fEnemyDir; break;
                case 3: enemyController.fEnemySpeed = 10 * enemyController.fEnemyDir; break;
            }
        }
    }
    public void StatChangeESize()
    {
        if (enemyController != null)
        {
            switch (statBlockUI.statsE[2]) //enemy sizes
            {
                // If enemy grows into wall, movement stops
                case 1:
                    enemyController.fEnemySize = 1.5f;
                    if (statBlockUI.prevSize != 1)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.81f);
                    }

                    break;
                case 2:
                    
                    switch (statBlockUI.prevSize)
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
                    if (statBlockUI.prevSize != 3)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.726443f);
                        enemyController.fEnemySize = 4.5f;
                    }

                    break;
            }
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(buildIndex + 1);
    }



}
