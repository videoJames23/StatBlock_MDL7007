using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    public bool bIsGrounded;
    private float fSpeed = 10;
    private float fJumpForce = 5;
    

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
        
        playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * fSpeed, playerRb.linearVelocity.y);
       if (Input.GetKeyDown("space") && bIsGrounded)
       {
           playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
       }
        
    }
    
    
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            bIsGrounded = true;
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            bIsGrounded = false;
    }
}
