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

    public float acceleration = 12f;
    public float airControlMultiplier = 0.5f;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    private StatBlockUI statBlockUI;
    private GameManager gameManagerScript;
    private InstructionManager instructionManagerScript;
    private SpriteRenderer cSpriteRenderer;
    public AudioController audioController;
    


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        GameObject instructionManager = GameObject.FindGameObjectWithTag("Instruction Manager");
        if (instructionManager)
        {
            instructionManagerScript = instructionManager.GetComponent<InstructionManager>();
        }
        
        GameObject audio =  GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController = audio.GetComponent<AudioController>();
        }
        
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.W) && bIsGrounded && !bInMenu)
        {
            Jump();
        
        }
        
    }

    void FixedUpdate()
    {

        if (!bInMenu)
        {
            float fInput = Input.GetAxisRaw("Horizontal");

            Vector2 vTargetVelocity = new Vector2(fInput * fPlayerSpeed, playerRb.linearVelocity.y);

            float fControl = bIsGrounded ? 1f : airControlMultiplier;

            playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, vTargetVelocity,acceleration * fControl * Time.fixedDeltaTime);
        }
    }

    public void Jump()
    {
        audioController.jumpSource.Play();
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fPlayerJump);
    }
    

    
    
    
    // Damage/I-Frames
    public void TakeDamage(int damage)
    {
        if (!bCanTakeDamage)
        {
            return;
        }
        
        bCanTakeDamage = false;
        
        audioController.damageSource.Play();
        
        statBlockUI.statsP[0] -= damage;
        statBlockUI.iPointsTotalP--;
            
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
<<<<<<< Updated upstream
        switch (other.gameObject.tag)
=======
        if (other.gameObject.CompareTag("Finish"))
        {  
            //Why is the playercontroller responsible for ending the level? This should probably be in a different script. -F
            Debug.Log("Level Complete!");
            audioController.completionSource.Play();
            yield return new WaitForSeconds(1.8f);
            gameManagerScript.LoadScene();
        }
        else if (instructionManagerScript)
>>>>>>> Stashed changes
        {
            case "Finish":
                Debug.Log("Level Complete!");
                completionSource.Play();
                yield return new WaitForSeconds(1.8f);
                gameManagerScript.LoadScene();
                break;
            
        }
        
        if (instructionManagerScript)
        {
            switch (other.gameObject.tag)
            {
                case "Text1":
                    instructionManagerScript.StartFadeIn("text1");
                    break;
                case "Text2":
                    instructionManagerScript.StartFadeIn("text2");
                    break;
                case "Text3":
                    instructionManagerScript.StartFadeIn("text3");
                    break;
                case "Text4":
                    instructionManagerScript.StartFadeIn("text4");
                    break;
            }
            
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
        switch (other.gameObject.tag)
        {
            case "StatBlockP":
                bIsTouchingStatBlockP = true;
                break;
            case  "StatBlockE":
                bIsTouchingStatBlockE = true;
                break;
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
        switch (other.gameObject.tag)
        {
            case "StatBlockP":
                bIsTouchingStatBlockP = false;
                break;
            case "StatBlockE":
                bIsTouchingStatBlockE = false;
                break;
        }
       
        if (instructionManagerScript)
        {
<<<<<<< Updated upstream
            switch (other.gameObject.tag)
=======
            bIsTouchingStatBlockE = false;
        }
        if (instructionManagerScript)
        {
            if (other.gameObject.CompareTag("Text1"))
>>>>>>> Stashed changes
            {
                case "Text1":
                    instructionManagerScript.StartFadeOut("text1");
                    break;
                case "Text2":
                    instructionManagerScript.StartFadeOut("text2");
                    break;
                case "Text3":
                    instructionManagerScript.StartFadeOut("text3");
                    break;
                case "Text4":
                    instructionManagerScript.StartFadeOut("text4");
                    break;
            }
            
        }

    }
    

}

