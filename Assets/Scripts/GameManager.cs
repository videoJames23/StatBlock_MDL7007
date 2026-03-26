using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCollisions  playerCollisions;
    
    public bool BInMenu { get; private set; }
    public bool BInMenuP { get; private set; }
    public bool BInMenuE { get; private set; }
    
    
    
    [SerializeField] private EnemyController enemyController;
    
    [SerializeField] private StatBlockUI statBlockUI;
    [SerializeField] private StatBlockChangesE statBlockChangesE;

    private int iBuildIndex;
     
    [SerializeField]  private PlayerStats  playerStats;
    [SerializeField]  private EnemyStats enemyStats;


    public delegate void MenuOpen();
    public static event MenuOpen OnMenuOpen;

    public delegate void MenuClose();
    public static event MenuClose OnMenuClose;

    public delegate void Error();
    public static event Error OnError;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerCollisions = player.GetComponent<PlayerCollisions>();

        GameObject enemy = GameObject.FindGameObjectWithTag("EnemyVisual");
        if (enemy)
        {
            enemyController = enemy.GetComponent<EnemyController>();
        }
        
        GameObject statBlockUIGO = GameObject.FindGameObjectWithTag("StatBlockUI");
        statBlockUI = statBlockUIGO.GetComponent<StatBlockUI>();
        statBlockChangesE = statBlockUI.GetComponent<StatBlockChangesE>();
        
    }
    
    private void OnEnable()
    {
        PlayerCollisions.OnCompletion += OnCompletion;
        StatBlockChangesP.OnDamageRefresh += DamageMenuRefreshP;
        StatBlockChangesE.OnDamageRefresh += DamageMenuRefreshE;
    }

    private void OnDisable()
    {
        PlayerCollisions.OnCompletion -= OnCompletion;
        StatBlockChangesP.OnDamageRefresh -= DamageMenuRefreshP;
        StatBlockChangesE.OnDamageRefresh -= DamageMenuRefreshE;
    }
    
    void Start()
    {
        iBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(iBuildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CollisionChecks();
        }
    }

    // CollisionChecks checks to see if the player is touching either StatBlock before opening the menu
    private void CollisionChecks()
    {
        if (playerCollisions.bIsTouchingStatBlockP)
        {
            BInMenuP = !BInMenuP;
        }
            
        if (playerCollisions.bIsTouchingStatBlockE)
        {
            if (!BInMenuE)
            {
                BInMenuE = true;
                enemyController.fPrevDir = enemyController.fEnemyDir;
                MenuChecks();
            }
            else if (BInMenuE)
            {
                if (statBlockChangesE.iPointsLeftE == 0)
                {
                    BInMenuE = false;
                    enemyController.fEnemyDir = enemyController.fPrevDir;
                    MenuChecks();
                }
                else if (statBlockChangesE.iPointsLeftE > 0)
                {
                    OnError?.Invoke();
                }
            }
        }
            
        MenuChecks();
        statBlockUI.UpdateUI();
    }
    
    private void MenuChecks()
    {
        if (BInMenuP || BInMenuE)
        {
            BInMenu = true;
            OnMenuOpen?.Invoke();
        }
        
        else
        {
            BInMenu = false;
            OnMenuClose?.Invoke();
        }
        MenuFreezeToggle();
    }
    
    void MenuFreezeToggle()
    {
        if (BInMenu)
        {
            Time.timeScale = 0;
        }
        else if (!BInMenu)
        {
            Time.timeScale = 1;
        }
    }
    
    // DamageMenuRefresh quickly updates the UI to display the stats of the entity that has just taken damage
    void DamageMenuRefreshP()
    {
        BInMenuP = true; 
        statBlockUI.UpdateUI(); 
        BInMenuP = false; 
        statBlockUI.UpdateUI();
    }

    void DamageMenuRefreshE()
    { 
        BInMenuE = true; 
        statBlockUI.UpdateUI(); 
        BInMenuE = false; 
        statBlockUI.UpdateUI();
    }

    private void OnCompletion()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
