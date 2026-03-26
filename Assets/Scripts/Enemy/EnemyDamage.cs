using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private EnemyStatsHandler enemyStatsHandler;
    
    private SpriteRenderer cSpriteRenderer;

    [SerializeField] private EnemyStats enemyStats;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    private bool bCanTakeDamage = true;

    public delegate void Damage();
    public static event Damage OnDamage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStatsHandler = GetComponent<EnemyStatsHandler>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        
        if (!bCanTakeDamage)
        {
            return;
        }
        
        OnDamage?.Invoke(); 
        bCanTakeDamage = false;
        
        if (enemyStatsHandler.runtimeStats.iEnemyHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
        
        else if (enemyStatsHandler.runtimeStats.iEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Invulnerability()
    {
        bCanTakeDamage = false;
        
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(1, 0.25f, 0, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        bCanTakeDamage = true;
    }
}
