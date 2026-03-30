using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyStats enemyStats;

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
        enemyRb.linearVelocity = new Vector2(enemyStatsHandler.runtimeStats.fEnemySpeed * enemyStatsHandler.runtimeStats.iEnemyDir, enemyRb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        enemyStatsHandler.runtimeStats.iEnemyDir *= -1;
    }
    
}


