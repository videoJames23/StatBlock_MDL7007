using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D enemyRb;
    private Rigidbody2D playerRb;
    private BoxCollider2D enemyCollider;
    public PlayerController playerController;

    public float fSpeed = 5;

    public int iEnemyHealth = 3;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyCollider =  GetComponent<BoxCollider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // find player
        playerRb = player.GetComponent<Rigidbody2D>(); // find player's rigidbody
        playerController = player.GetComponent<PlayerController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.linearVelocity = new Vector2(1 * fSpeed, enemyRb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            fSpeed *= -1;
        }

        if (other.gameObject.tag == "Player")
        {  
            // // alternate jump detector using maths :3
            // // ((px x ex) + (py + ey))/|p||e| = cosangle
            // Vector2 enemyPosition = enemyRb.transform.position;
            // Vector2 relativePlayerPosition = enemyRb.transform.position - playerRb.transform.position;
            // float enemyMag = enemyPosition.magnitude;
            // float playerMag = relativePlayerPosition.magnitude;
            // float magProduct = enemyMag * playerMag;
            // float playerCosAngle =
            //     ((relativePlayerPosition.x * enemyPosition.x) + (relativePlayerPosition.y * enemyPosition.y)) /
            //     magProduct;
            // if (playerCosAngle <= 0.525)
            // {
            //     Debug.Log("Squash!");
            //     iEnemyHealth--;
            //     float fJumpForce = playerController.fJumpForce;
            //     playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
            //     if (iEnemyHealth <= 0)
            //     {
            //         Destroy(gameObject);
            //     }
            // }
            //
            // else if (playerCosAngle > 0.525)
            // {
            //     Debug.Log("Ow!");
            //     playerController.TakeDamage(1);
            // }
            
            ContactPoint2D contact = other.contacts[0];
            
            bool playerIsAbove = contact.normal.y < -0.5f && playerRb.linearVelocity.y <= 0f;
            
            if (playerIsAbove)
            {
                Debug.Log("Squash!");
                iEnemyHealth--;
                float fJumpForce = playerController.fJumpForce;
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
                if (iEnemyHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("Ow!");
                playerController.TakeDamage(1);
            }

        }
    }
}

