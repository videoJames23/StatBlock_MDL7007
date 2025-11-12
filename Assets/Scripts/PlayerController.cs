using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    public bool bIsGrounded;
    public float fSpeed = 10;
    public float fJumpForce = 5;
    public int iPlayerHealth = 3;
 

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

        playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * fSpeed, playerRb.linearVelocity.y); // get horizontal movement info
        
        if (Input.GetKeyDown("space") && bIsGrounded) // Jump
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
        }

    }

    public void TakeDamage(int damage)
    {
        iPlayerHealth -= damage;
        
        
        
        if (iPlayerHealth <= 0)
        {
            Destroy(this.gameObject);
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
