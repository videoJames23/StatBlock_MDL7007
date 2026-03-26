using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    [SerializeField]private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    private EnemyDamage enemyDamage;
    [SerializeField] private EnemyStats enemyStats;
    private EnemyStatsHandler enemyStatsHandler;
    
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D playerRb;
    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyDamage = GetComponent<EnemyDamage>();
        enemyStatsHandler = GetComponent<EnemyStatsHandler>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerDamage = player.GetComponent<PlayerDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
            if (Mathf.Abs(enemyRb.linearVelocity.x) < 0.1f)
            {
                enemyController.fEnemyDir *= -1;
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {  
            
            // MATHS CONTENT HERE
            // ((px x ex) + (py + ey))/|p||e| = cosangle
            Vector2 toPlayer = playerRb.position - enemyRb.position;
            float upMag = Vector2.up.magnitude;
            float toPlayerMag = toPlayer.magnitude;
            float magProduct = toPlayerMag * upMag;
            float playerCosAngle =
                ((toPlayer.x * Vector2.up.x) + (toPlayer.y * Vector2.up.y)) /
                magProduct;
            bool bPlayerIsFalling = playerRb.linearVelocity.y <= 0f;
            
           
            if (playerCosAngle > enemyStats.fCosAngle && bPlayerIsFalling)
            {
                Debug.Log("Squash!");
                enemyDamage.TakeDamage();
                playerMovement.bIsGrounded = true;
                playerMovement.Jump();
            }
            
            else if (playerCosAngle < enemyStats.fCosAngle || !bPlayerIsFalling)
            {
                Debug.Log("Ow!");
                playerDamage.TakeDamage();
            }
        }

        if (other.gameObject.CompareTag("Spike"))
        {
            enemyDamage.TakeDamage();
            
            enemyController.fEnemyDir *= -1;
                
            if (enemyStatsHandler.runtimeStats.iEnemyHealth > 0)
            {
                StartCoroutine(enemyDamage.Invulnerability());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
           enemyDamage.TakeDamage();
           
        }
        
    }
}
