using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    
    
    [FormerlySerializedAs("fSpeed")] [Header("Stats")]
    public int iPlayerHealth;
    public float fPlayerSpeed;
    public float fPlayerJump;
    
    
    
    public bool bIsTouchingStatBlockP;
    public bool bIsTouchingStatBlockE;
    public bool bInMenu;
    public bool bInMenuP;
    public bool bInMenuE;
    private bool bIsGrounded;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    private StatBlockUI statBlockUI;
    private EnemyController enemyController;
    private SpriteRenderer cSpriteRenderer;
    


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject statBlockP = GameObject.FindGameObjectWithTag("StatBlockP");
        statBlockUI = statBlockP.GetComponent<StatBlockUI>();
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyController = enemy.GetComponent<EnemyController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (bIsTouchingStatBlockP && Input.GetKeyDown(KeyCode.E))
        {
            bInMenuP = !bInMenuP;
            statBlockUI.UpdateUI();
        }
        
        if (bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E) && !bInMenuE)
        {
            bInMenuE = true;
            statBlockUI.UpdateUI();
        }
        else if (bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E) && bInMenuE && statBlockUI.iPointsLeftE == 0)
        {
            bInMenuE = false;
        }
        if (bInMenuP || bInMenuE)
        {
            bInMenu = true;

        }
        else if (!bInMenuP && !bInMenuE)
        {
            bInMenu = false;
            
        }
        
            
    
        
        playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * fPlayerSpeed, playerRb.linearVelocity.y);
        
        iPlayerHealth = statBlockUI.statsP[0];
        
        if (Input.GetKeyDown("space") && bIsGrounded)
        {
            Jump();
        }

    }

    public void Jump()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fPlayerJump);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {  
            Debug.Log("Level Complete!");
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            bIsGrounded = true;
        }
        
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StatBlockP"))
        {
            bIsTouchingStatBlockP = true;
        }
        else if (other.gameObject.CompareTag("StatBlockE"))
        {
            bIsTouchingStatBlockE = true;
        }
    }
   
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            bIsGrounded = false;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StatBlockP"))
        {
            bIsTouchingStatBlockP = false;
        }
        if (other.gameObject.CompareTag("StatBlockE"))
        {
            bIsTouchingStatBlockE = false;
        }
    }
    
    
    // Damage/I-Frames
    public void TakeDamage(int damage)
        {
            iPlayerHealth -= damage;
            statBlockUI.statsP[0]--;
            statBlockUI.iPointsTotalP--;
            statBlockUI.UpdateUI();
            // I-frames
            if (iPlayerHealth > 0)
            {
                StartCoroutine(Invulnerability());
            }
            
            else if (iPlayerHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        
      
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(0, 0.25f, 1, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
