using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private StatBlockUI statBlockUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockP = GameObject.FindGameObjectWithTag("StatBlockP");
        statBlockUI = statBlockP.GetComponent<StatBlockUI>();
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyRb =  enemy.GetComponent<Rigidbody2D>();
        enemyController = enemy.GetComponent<EnemyController>();
        
        StatChangeP();
        StatChangeE();
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (playerController == null)
        {
                
        }

        else
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

    public void StatChangeP()
    {
        Debug.Log("StatChangeP");
        if (playerController == null)
        {
            
        }
        else
        {


            switch (statBlockUI.statsP[1]) // player speeds
            {
                case 0: playerController.fPlayerSpeed = 0; break;
                case 1: playerController.fPlayerSpeed = 3; break;
                case 2: playerController.fPlayerSpeed = 7; break;
                case 3: playerController.fPlayerSpeed = 10; break;
            }

            switch (statBlockUI.statsP[2]) //player jump heights
            {
                case 0: playerController.fPlayerJump = 0; break;
                case 1: playerController.fPlayerJump = 5; break;
                case 2: playerController.fPlayerJump = 7; break;
                case 3: playerController.fPlayerJump = 9; break;
            }
        }
    }

    public void StatChangeE()
    {
        Debug.Log("StatChangeE");
        if (enemyController == null)
        {
            
        }

        else
        {
            switch (statBlockUI.statsE[1]) // enemy speeds
            {
                case 0: enemyController.fEnemySpeed = 0; break;
                case 1: enemyController.fEnemySpeed = 3 * enemyController.fEnemyDir; break;
                case 2: enemyController.fEnemySpeed = 7 * enemyController.fEnemyDir; break;
                case 3: enemyController.fEnemySpeed = 10 * enemyController.fEnemyDir; break;
            }
            
            
            switch (statBlockUI.statsE[2]) //enemy sizes
            {
                // If enemy grows into wall, movement stops
                case 1: enemyController.fEnemySize = 1.5f; break;
                case 2: enemyController.fEnemySize = 3f; break;
                case 3: enemyController.fEnemySize = 4.5f; break;
            }
            
        }

    }
    

    
}
