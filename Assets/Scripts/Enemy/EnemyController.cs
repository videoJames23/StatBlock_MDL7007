using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        // Responsible for:
        // moving enemy left to right
        // &
        // flipping direction when instructed
        
        [SerializeField] private Rigidbody2D enemyRb;
        [SerializeField] private EnemyStatValues enemyStats;
        [SerializeField] private EnemyStatsHandler enemyStatsHandler;
        
        private void OnEnable()
        {
            EnemyCollisions.OnEnemySpike += FlipDirection;
            EnemyCollisions.OnEnemyWall += FlipDirection;
        }

        private void OnDisable()
        {
            EnemyCollisions.OnEnemySpike -= FlipDirection;
            EnemyCollisions.OnEnemyWall -= FlipDirection;
        }
    
    
        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!enemyRb || !enemyStats) return;
            var stats = enemyStatsHandler.runtimeStats;
            enemyRb.linearVelocity = new Vector2(stats.enemySpeed * stats.enemyDir, enemyRb.linearVelocity.y);
        }

        private void FlipDirection()
        {
            enemyStatsHandler.runtimeStats.enemyDir *= -1;
        }
    }
}


