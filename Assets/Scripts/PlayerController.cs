using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private float h;
    private bool isGrounded;
    private bool canJump;
    private float speed = 10;
    private float jumpForce = 5;
    

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerRb.linearVelocity.y);
       if (Input.GetKeyDown("space"))
       {
           playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
       }
        
    }
}
