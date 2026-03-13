
using UnityEngine;




public class EnemyController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyStats enemyStats;
    
    public float fEnemyDir;
    public float fPrevDir;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!enemyRb || !enemyStats) return;
        enemyRb.linearVelocity = new Vector2(enemyStats.fEnemySpeed * fEnemyDir, enemyRb.linearVelocity.y);
    }

    
    
}


