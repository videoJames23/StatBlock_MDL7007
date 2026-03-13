using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //serializedfield stuff here again -F
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private PlayerCollisions  playerCollisions;
    
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    
    private StatBlockUI statBlockUI;
    private StatBlockChanges statBlockChanges;
    
    
    
    public int iBuildIndex;
    
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats enemyStats;

    
    private Vector2 vEnemyVelocity;


    public delegate void MenuOpen();
    public static event MenuOpen OnMenuOpen;

    public delegate void MenuClose();
    public static event MenuClose OnMenuClose;

    public delegate void Error();
    public static event Error OnError;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        playerCollisions = player.GetComponent<PlayerCollisions>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        statBlockChanges = statBlockUI.GetComponent<StatBlockChanges>();
        
        
        iBuildIndex = SceneManager.GetActiveScene().buildIndex;
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy)
        {
            enemyRb = enemy.GetComponent<Rigidbody2D>();
            enemyController = enemy.GetComponent<EnemyController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //there has to be a better way to do this that doesn't involve this many if/else statements -F
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(iBuildIndex);
        }
        
        if (playerCollisions.bIsTouchingStatBlockP && Input.GetKeyDown(KeyCode.E))
        {
            playerController.bInMenuP = !playerController.bInMenuP;
            
            MenuChecks();
            MenuFreezeToggle();
        }
        
        if (playerCollisions.bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E))
        {
            if (!playerController.bInMenuE)
            {
                playerController.bInMenuE = true;
                enemyController.fPrevDir = enemyController.fEnemyDir;
                MenuChecks();
            }
            else if (playerController.bInMenuE)
            {
                if (statBlockChanges.iPointsLeftE == 0)
                {
                    playerController.bInMenuE = false;
                    enemyController.fEnemyDir = enemyController.fPrevDir;
                    MenuChecks();
                }
                else if (statBlockChanges.iPointsLeftE > 0)
                {
                    OnError?.Invoke();
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
            OnMenuOpen?.Invoke();
        }
        
        else
        {
            playerController.bInMenu = false;
            OnMenuClose?.Invoke();
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
    
    

    public void LoadScene()
    {
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
