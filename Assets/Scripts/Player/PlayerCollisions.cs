using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerDamage playerDamage;
    
    
    public bool BIsTouchingStatBlockP{ get; private set; }
    public bool BIsTouchingStatBlockE{ get; private set; }

    public delegate void Completion();
    public static event Completion OnCompletion;

    public delegate void Ground();
    public static event Ground OnGround;
    public delegate void UnGround();
    public static event UnGround OnUnGround;
    void Start()
    {
        playerDamage = GetComponent<PlayerDamage>();
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
                playerDamage.TakeDamage();
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
            OnGround?.Invoke();
        }
        
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "StatBlockP":
                BIsTouchingStatBlockP = true;
                break;
            case "StatBlockE":
                BIsTouchingStatBlockE = true;
                break;
        }
        
        
    }
   
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            OnUnGround?.Invoke();
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "StatBlockP":
                BIsTouchingStatBlockP = false;
                break;
            case "StatBlockE":
                BIsTouchingStatBlockE = false;
                break;
        }
        
       

    }
}
