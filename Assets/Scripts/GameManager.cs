using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //serializedfield stuff here again -F
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private PlayerCollisions  playerCollisions;
    
    [SerializeField]private Rigidbody2D enemyRb;
    [SerializeField]private EnemyController enemyController;
    
    private StatBlockUI statBlockUI;
    private StatBlockChangesP statBlockChangesP;
    private StatBlockChangesE statBlockChangesE;
    
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
        statBlockChangesP = statBlockUI.GetComponent<StatBlockChangesP>();
        statBlockChangesE = statBlockUI.GetComponent<StatBlockChangesE>();
        
        
        iBuildIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        //there has to be a better way to do this that doesn't involve this many if/else statements -F
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
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
                if (statBlockChangesE.iPointsLeftE == 0)
                {
                    playerController.bInMenuE = false;
                    enemyController.fEnemyDir = enemyController.fPrevDir;
                    MenuChecks();
                }
                else if (statBlockChangesE.iPointsLeftE > 0)
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
            Time.timeScale = 0;

        }
        else if (!playerController.bInMenu)
        {
            Time.timeScale = 1;
                
        }
    }
    
    

    public void LoadScene()
    {
        
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
