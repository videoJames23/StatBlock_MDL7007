using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private GameManager gameManagerScript;
    
    public PlayerController playerController;

    private EnemyStatsHandler enemyStatsHandler;
   
    private StatBlockChanges statBlockChanges;
    private StatBlockUI statBlockUI;
    
    private SpriteRenderer cSpriteRenderer;

    [SerializeField] private EnemyStats enemyStats;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;

    public delegate void Damage();
    public static event Damage OnDamage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatsHandler = GetComponent<EnemyStatsHandler>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        statBlockChanges = statBlockUI.GetComponent<StatBlockChanges>();

        enemyStats.iDamage = 1;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        OnDamage?.Invoke();
        
        statBlockChanges.statsE[0] -= damage;
        
        if (statBlockChanges.iPointsLeftE > 0)
        {
            statBlockChanges.iPointsLeftE--;
        }
        
        statBlockChanges.iPointsTotalE--;
        
        statBlockChanges.StatChangeEHealth();

        
        playerController.bInMenuE = true;
        statBlockUI.UpdateUI();
        playerController.bInMenuE = false;
        statBlockUI.UpdateUI();
        
        if (enemyStatsHandler.runtimeStats.iEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Invulnerability()
    {
        enemyStats.iDamage = 0;
        
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(1, 0.25f, 0, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        enemyStats.iDamage = 1;
    }
}
