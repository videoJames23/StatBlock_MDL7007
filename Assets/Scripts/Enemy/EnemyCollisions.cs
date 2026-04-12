using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Enemy
{
    public class EnemyCollisions : MonoBehaviour
    {
        // Responsible for:
        // detecting enemy collisions
        // &
        // informing listeners of collisions
        // &
        // determining if player or enemy should take damage on collision
        
        [Header("Enemy")]
        [SerializeField] private EnemyStatValues enemyStats;
        [SerializeField] private Rigidbody2D enemyRb;
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private EnemyDamage enemyDamage;
        
        [Header("Player")]
        [SerializeField] private Rigidbody2D playerRb;

        
        // Events
        public delegate void PlayerSquash();
        public static event PlayerSquash OnPlayerSquash;

        public delegate void EnemyAttack();

        public static event EnemyAttack OnEnemyAttack;

        public delegate void EnemySpike();
        public static event EnemySpike OnEnemySpike;

        public delegate void EnemyWall();
        public static event EnemyWall OnEnemyWall;
        
        
        // Collisions
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                if (Mathf.Abs(enemyRb.linearVelocity.x) < 0.1f)
                {
                    OnEnemyWall?.Invoke();
                    // Enemy flips direction
                }
            }

            if (other.gameObject.CompareTag("Player"))
            {  
                // MATHS CONTENT HERE
                // ((px x ex) + (py + ey))/|p||e| = cos angle
                var toPlayer = playerRb.position - (Vector2)enemyTransform.position;
                var upMag = Vector2.up.magnitude;
                var toPlayerMag = toPlayer.magnitude;
                var magProduct = toPlayerMag * upMag;
                var playerCosAngle =
                    ((toPlayer.x * Vector2.up.x) + (toPlayer.y * Vector2.up.y)) /
                    magProduct;
                var bPlayerIsFalling = playerRb.linearVelocity.y <= 0f;
            
           
                if (playerCosAngle > enemyStats.cosAngle && bPlayerIsFalling)
                {
                    Debug.Log("Squash!");
                    OnPlayerSquash?.Invoke();
                    // Player lands on enemy, does damage, and jumps off
                }
            
                else if (playerCosAngle < enemyStats.cosAngle || !bPlayerIsFalling)
                {
                    Debug.Log("Ow!");
                    OnEnemyAttack?.Invoke();
                    // Enemy collides with player and takes damage
                }
            }

            if (other.gameObject.CompareTag("Spike"))
            {
                OnEnemySpike?.Invoke();
                // Enemy takes damage and flips direction
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Spike") && enemyDamage.CanTakeDamage)
            {
                OnEnemySpike?.Invoke();
                // Enemy takes damage and flips direction
            }
        
        }
    }
}
