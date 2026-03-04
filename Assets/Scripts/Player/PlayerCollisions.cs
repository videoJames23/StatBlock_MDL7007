using System.Collections;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;
    private AudioController audioController;
    private GameManager gameManagerScript;
    
    public bool bIsTouchingStatBlockP;
    public bool bIsTouchingStatBlockE;
    void Start()
    {
        playerDamage = GetComponent<PlayerDamage>();
        playerMovement = GetComponent<PlayerMovement>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        
        GameObject audio = GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController = audio.GetComponent<AudioController>();
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
                playerDamage.TakeDamage(1);
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
        
       

    }
}
