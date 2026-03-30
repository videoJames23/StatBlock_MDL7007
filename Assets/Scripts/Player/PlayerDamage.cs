using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    private bool bCanTakeDamage = true;
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    private SpriteRenderer cSpriteRenderer;
    
    private PlayerStatsHandler playerStatsHandler;
    
    public delegate void Damage();
    public static event  Damage OnDamage;

    private void OnEnable()
    {
        EnemyCollisions.OnEnemyAttack += TakeDamage;
    }

    private void OnDisable()
    {
        EnemyCollisions.OnEnemyAttack -= TakeDamage;
    }
        
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        
        cSpriteRenderer = GetComponent<SpriteRenderer>(); 
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    
    public void TakeDamage()
    {
        if (!bCanTakeDamage)
        {
            return;
        }
        
        OnDamage?.Invoke(); 
        bCanTakeDamage = false;
        
        if (playerStatsHandler.runtimeStats.iPlayerHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
        
        else if (playerStatsHandler.runtimeStats.iPlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        
      
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(0, 0.25f, 1, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
        bCanTakeDamage = true;
    }
}
