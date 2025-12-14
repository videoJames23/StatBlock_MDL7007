using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class EnemyController : MonoBehaviour
{
    private GameManager gameManagerScript;
    private Rigidbody2D enemyRb;
    private Rigidbody2D playerRb;
    public PlayerController playerController;
 
    private SpriteRenderer cSpriteRenderer;

    
    public int iEnemyHealth;
    public float fEnemySpeed;
    public float fEnemySize;
    public float fEnemyDir;
    public float fPrevDir;
    public int iDamage;
    public float cosAngle = 0.70710678118f;
    
    private StatBlockUI statBlockUI;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        
        enemyRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();

        iDamage = 1;


    }

    // Update is called once per frame
    void Update()
    {   
        enemyRb.linearVelocity = new Vector2(fEnemySpeed * fEnemyDir, enemyRb.linearVelocity.y);
        enemyRb.transform.localScale = new Vector2(fEnemySize, fEnemySize);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (Mathf.Abs(enemyRb.linearVelocity.x) < 0.1f)
            {
                fEnemyDir *= -1;
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
            
           
            if (playerCosAngle > cosAngle && bPlayerIsFalling)
            {
                Debug.Log("Squash!");
                TakeDamage(1);
                playerController.Jump();
                if (iEnemyHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
            
            else if (playerCosAngle < cosAngle || !bPlayerIsFalling)
            {
                Debug.Log("Ow!");
                playerController.TakeDamage(1);
            }
            
            
            // easier collision detection
            // ContactPoint2D contact = other.contacts[0];
            // if (playerRb != null) 
            // {
            //     bool playerIsAbove = contact.normal.y < -0.5f && playerRb.linearVelocity.y <= 0f;
            //     if (playerIsAbove)
            //     {
            //         Debug.Log("Squash!");
            //         TakeDamage(iDamage);
            //         playerController.Jump();
            //         if (iEnemyHealth > 0)
            //         {
            //             StartCoroutine(Invulnerability());
            //         }
            //
            //     }
            //     else
            //     {
            //         Debug.Log("Ow!");
            //         playerController.TakeDamage(1);
            //     }
            // }
            //
            
            

        }

        if (other.gameObject.CompareTag("Spike"))
        {
            TakeDamage(iDamage);
            playerController.damageSource.Play();
            fEnemyDir *= -1;
                
            if (iEnemyHealth > 0)
            {
                StartCoroutine(Invulnerability());
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
        
        gameManagerScript.StatChangeEHealth();

        
        playerController.bInMenuE = true;
        statBlockUI.UpdateUI();
        playerController.bInMenuE = false;
        statBlockUI.UpdateUI();
        
        

        // I-frames
        if (iEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Invulnerability()
    {
        iDamage = 0;
        
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(1, 0.25f, 0, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        iDamage = 1;
    }
    

    
}


