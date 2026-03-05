using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    private EnemyController enemyController;
    [SerializeField] private EnemyStats enemyStats;
    
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb =  GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
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
                enemyController.TakeDamage(1);
                playerMovement.bIsGrounded = true;
                playerMovement.Jump();
                if (enemyStats.iEnemyHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
            
            else if (playerCosAngle < enemyStats.fCosAngle || !bPlayerIsFalling)
            {
                Debug.Log("Ow!");
                playerDamage.TakeDamage(1);
            }
        }

        if (other.gameObject.CompareTag("Spike"))
        {
            enemyController.TakeDamage(enemyStats.iDamage);
            
            enemyController.fEnemyDir *= -1;
                
            if (enemyStats.iEnemyHealth > 0)
            {
                StartCoroutine(enemyController.Invulnerability());
            }
        }
    }
}
