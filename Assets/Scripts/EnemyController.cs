using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    private Rigidbody2D enemyRb;

    private int iSpeed = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb =  GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.linearVelocity = new Vector2(1 * iSpeed, enemyRb.linearVelocity.y);
    }
}
