using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;
    
    
    public bool bIsTouchingStatBlockP;
    public bool bIsTouchingStatBlockE;

    public delegate void Completion();
    public static event Completion OnCompletion;
    void Start()
    {
        playerDamage = GetComponent<PlayerDamage>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
                playerDamage.TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (other.gameObject.tag)
        
        {  
            case "Finish":
            Debug.Log("Level Complete!");
            OnCompletion?.Invoke();
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
            case "StatBlockE":
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
