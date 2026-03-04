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
    
    [SerializeField] private PlayerStats  playerStats;
    
    public bool bIsTouchingStatBlockP;
    public bool bIsTouchingStatBlockE;
    public bool bInMenu;
    public bool bInMenuP;
    public bool bInMenuE;
    
    
    private bool bCanTakeDamage = true;

    public float fInput;
    
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    
    private StatBlockUI statBlockUI;
    private GameManager gameManagerScript;
    private InstructionManager instructionManagerScript;
    private SpriteRenderer cSpriteRenderer;
    public AudioController audioController;
    public PlayerMovement playerMovement;


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        
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
        
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !bInMenu)
        {
            playerMovement.Jump();
        }
        
    }

    public void FixedUpdate()
    {

        if (!bInMenu)
        {
            fInput = Input.GetAxisRaw("Horizontal");
        }
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
        if (playerStats.iPlayerHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
            
        else if (playerStats.iPlayerHealth <= 0)
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
            //might be a way to just target the alpha channel instead of the whole colour,
            //which would let you change the player's colour without having to adjust it here too -F
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

        switch (other.gameObject.tag)
        
        {  case "Finish":
            //Why is the playercontroller responsible for ending the level? This should probably be in a different script. -F
            Debug.Log("Level Complete!");
            audioController.completionSource.Play();
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
            playerMovement.bIsGrounded = true;
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
            playerMovement.bIsGrounded = false;
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
        {switch (other.gameObject.tag)

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

