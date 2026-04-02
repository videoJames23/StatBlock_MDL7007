using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private PlayerStatValues  playerStats;
    private bool canTakeDamage = true;
    private float iFramesDuration = 1;
    private int numberOfFlashes = 5;
    private SpriteRenderer spriteRenderer;
    
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
        
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    
    public void TakeDamage()
    {
        if (!canTakeDamage)
        {
            return;
        }
        
        OnDamage?.Invoke(); 
        canTakeDamage = false;
        
        if (playerStatsHandler.runtimeStats.playerHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
        
        else if (playerStatsHandler.runtimeStats.playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        
      
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(0, 0.25f, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
            spriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
        canTakeDamage = true;
    }
}
