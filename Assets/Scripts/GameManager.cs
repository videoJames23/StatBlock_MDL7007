using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public Rigidbody2D enemyRb;
    public Rigidbody2D playerRb;
    public PlayerController playerController;
    
    
    [Header("Player Stats")]
    public float fSpeed = 10;
    public float fJumpForce = 10f;
    public int iPlayerHealth = 3;
    
    [Header("Enemy Stats")]
    public int iEnemySize = 2;
    public float fEnemySpeed = 5;
    public int iEnemyHealth = 3;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
