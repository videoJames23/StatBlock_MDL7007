using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private EnemyStatsHandler enemyStatsHandler;
    
    private SpriteRenderer cSpriteRenderer;

    [SerializeField] private EnemyStats enemyStats;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    public bool BCanTakeDamage{ get; private set; }

    public delegate void Damage();
    public static event Damage OnDamage;

    private void OnEnable()
    {
        EnemyCollisions.OnEnemySpike += TakeDamage;
        EnemyCollisions.OnPlayerSquash += TakeDamage;
    }
    
    private void OnDisable()
    {
        EnemyCollisions.OnEnemySpike -= TakeDamage;
        EnemyCollisions.OnPlayerSquash -= TakeDamage;
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatsHandler = GetComponent<EnemyStatsHandler>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        BCanTakeDamage = true;
    }
    
    public void TakeDamage()
    {
        
        if (!BCanTakeDamage)
        {
            return;
        }
        
        OnDamage?.Invoke(); 
        BCanTakeDamage = false;
        
        if (enemyStatsHandler.runtimeStats.iEnemyHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
        
        else if (enemyStatsHandler.runtimeStats.iEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Invulnerability()
    {
        BCanTakeDamage = false;
        
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(1, 0.25f, 0, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        BCanTakeDamage = true;
    }
}
