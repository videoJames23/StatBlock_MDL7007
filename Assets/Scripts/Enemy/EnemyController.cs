using UnityEngine;
public class EnemyController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] private EnemyStatsHandler enemyStatsHandler;
    
    public float fEnemyDir;
    public float fPrevDir;
    
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!enemyRb || !enemyStats) return;
        enemyRb.linearVelocity = new Vector2(enemyStatsHandler.runtimeStats.fEnemySpeed * fEnemyDir, enemyRb.linearVelocity.y);
    }

    
    
}


