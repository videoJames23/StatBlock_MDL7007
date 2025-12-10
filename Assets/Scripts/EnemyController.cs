using System;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D enemyRb;
    private Rigidbody2D playerRb;
    public PlayerController playerController;
 

    
    public int iEnemyHealth;
    public float fEnemySpeed;
    public float fEnemySize;
    public float fEnemyDir;
    
    private StatBlockUI statBlockUI;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        

    }

    // Update is called once per frame
    void Update()
    {   
        enemyRb.linearVelocity = new Vector2(fEnemySpeed * fEnemyDir, enemyRb.linearVelocity.y);
        enemyRb.transform.localScale = new Vector2(fEnemySize, fEnemySize);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (Mathf.Abs(enemyRb.linearVelocity.x) < 0.1f)
            {
                fEnemyDir *= -1;
            }
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
            if (playerRb == null)
            {
                
            }

            else
            {
                bool playerIsAbove = contact.normal.y < -0.5f && playerRb.linearVelocity.y <= 0f;
                if (playerIsAbove)
                {
                    Debug.Log("Squash!");
                    TakeDamage(1);
                    playerController.Jump();

                }
                else
                {
                    Debug.Log("Ow!");
                    playerController.TakeDamage(1);
                }
            }
            
            
            

        }
    }
    void TakeDamage(int damage)
    {
        iEnemyHealth -= damage;
        statBlockUI.statsE[0]--;
        
        if (statBlockUI.iPointsLeftE > 0)
        {
            statBlockUI.iPointsLeftE--;
        }
        statBlockUI.iPointsTotalE--;
        statBlockUI.iPointsLeftE = statBlockUI.iPointsTotalE - statBlockUI.statsE.Sum();
        if (playerController == null)
        {
                
        }

        else
        {
            playerController.bInMenuE = true;
            statBlockUI.UpdateUI();
            playerController.bInMenuE = false;
        }

        // I-frames
        if (iEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}

