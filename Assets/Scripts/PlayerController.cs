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
    private bool bCanTakeDamage = true;

    
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    private StatBlockUI statBlockUI;
    private GameManager gameManagerScript;
    private InstructionManager instructionManagerScript;
    private EnemyController enemyController;
    private SpriteRenderer cSpriteRenderer;
    
    private AudioSource completionSource;
    private AudioSource jumpSource;
    public AudioSource damageSource;
    private AudioSource openSource;
    private AudioSource closeSource;
    


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            enemyController = enemy.GetComponent<EnemyController>();
        }
        
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject completionAudio = GameObject.FindGameObjectWithTag("Completion Audio");
        if (completionAudio != null)
        {
            completionSource = completionAudio.GetComponent<AudioSource>();
        }
        GameObject jumpAudio = GameObject.Find("Jump");
        if (jumpAudio != null)
        {
            jumpSource = jumpAudio.GetComponent<AudioSource>();
        }
        
        GameObject damageAudio = GameObject.Find("Damage");
        if (damageAudio != null)
        {
            damageSource = damageAudio.GetComponent<AudioSource>();
        }
        
        GameObject openAudio = GameObject.Find("Open");
        if (openAudio != null)
        {
            openSource = openAudio.GetComponent<AudioSource>();
        }
        
        GameObject closeAudio = GameObject.Find("Close");
        if (closeAudio != null)
        {
            closeSource = closeAudio.GetComponent<AudioSource>();
        }
        
        GameObject instructionManager = GameObject.FindGameObjectWithTag("Instruction Manager");
        if (instructionManager != null)
        {
            instructionManagerScript = instructionManager.GetComponent<InstructionManager>();
        }
        
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (bIsTouchingStatBlockP && Input.GetKeyDown(KeyCode.E))
        {
            bInMenuP = !bInMenuP;
            if (bInMenuP)
            {
                openSource.Play();
            }
            else if (!bInMenuP)
            {
                closeSource.Play();
            }
            MenuChecks();
        }
        
        if (bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E) && !bInMenuE)
        {
            bInMenuE = true;
            openSource.Play();
            enemyController.fPrevDir = enemyController.fEnemyDir;
            MenuChecks();
        }
        
        else if (bIsTouchingStatBlockE && Input.GetKeyDown(KeyCode.E) && bInMenuE && statBlockUI.iPointsLeftE == 0)
        {
            bInMenuE = false;
            closeSource.Play();
            enemyController.fEnemyDir = enemyController.fPrevDir;
            MenuChecks();
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
        jumpSource.Play();
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fPlayerJump);
    }
    

    void MenuChecks()
    {
        if (bInMenuP || bInMenuE)
        {
            bInMenu = true;
        }
        
        else if (!bInMenuP && !bInMenuE)
        {
            bInMenu = false;
        }
        statBlockUI.UpdateUI();
    }
    
    
    // Damage/I-Frames
    public void TakeDamage(int damage)
    {
        if (!bCanTakeDamage)
        {
            return;
        }
        
        bCanTakeDamage = false;
        
        damageSource.Play();
        
        iPlayerHealth -= damage;
        statBlockUI.statsP[0]--;
            
        statBlockUI.iPointsTotalP--;
        statBlockUI.iPointsLeftP = statBlockUI.iPointsTotalP - statBlockUI.statsP.Sum();
            
        gameManagerScript.StatChangePHealth();
        bInMenuP = true;
        statBlockUI.UpdateUI();
        bInMenuP = false;
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
        bCanTakeDamage = true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
                TakeDamage(1);
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {  
            Debug.Log("Level Complete!");
            completionSource.Play();
            yield return new WaitForSeconds(4);
            gameManagerScript.LoadScene();
        }
        else if (other.gameObject.CompareTag("Text1"))
        {
            instructionManagerScript.StartFadeIn("text1");
        }
        else if (other.gameObject.CompareTag("Text2"))
        {
            instructionManagerScript.StartFadeIn("text2");
        }
        else if (other.gameObject.CompareTag("Text3"))
        {
            instructionManagerScript.StartFadeIn("text3");
        }
        else if (other.gameObject.CompareTag("Text4"))
        {
            instructionManagerScript.StartFadeIn("text4");
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
        
        else if (other.gameObject.CompareTag("Text1"))
        {
            instructionManagerScript.StartFadeOut("text1");
        }
        else if (other.gameObject.CompareTag("Text2"))
        {
            instructionManagerScript.StartFadeOut("text2");
        }
        else if (other.gameObject.CompareTag("Text3"))
        {
            instructionManagerScript.StartFadeOut("text3");
        }
        else if (other.gameObject.CompareTag("Text4"))
        {
            instructionManagerScript.StartFadeOut("text4");
        }
        
    }
    

}

