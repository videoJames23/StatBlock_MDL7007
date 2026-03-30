using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    [SerializeField]private Rigidbody2D enemyRb;
    private EnemyDamage enemyDamage;
    [SerializeField] private EnemyStats enemyStats;
    
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D playerRb;

    public delegate void PlayerSquash();
    public static event PlayerSquash OnPlayerSquash;

    public delegate void EnemyAttack();

    public static event EnemyAttack OnEnemyAttack;

    public delegate void EnemySpike();
    public static event EnemySpike OnEnemySpike;

    public delegate void EnemyWall();

    public static event EnemyWall OnEnemyWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
            if (Mathf.Abs(enemyRb.linearVelocity.x) < 0.1f)
            {
                OnEnemyWall?.Invoke();
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
                OnPlayerSquash?.Invoke();
            }
            
            else if (playerCosAngle < enemyStats.fCosAngle || !bPlayerIsFalling)
            {
                Debug.Log("Ow!");
                OnEnemyAttack?.Invoke();
            }
        }

        if (other.gameObject.CompareTag("Spike"))
        {
            OnEnemySpike?.Invoke();
            
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike") && enemyDamage.BCanTakeDamage)
        {
            OnEnemySpike?.Invoke();
        }
        
    }
}
