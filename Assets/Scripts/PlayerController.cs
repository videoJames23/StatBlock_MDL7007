using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    
    [Header("Stats")]
    public float fSpeed = 10;
    public float fJumpForce = 10f;
    public int iPlayerHealth = 3;
    
    private bool bIsGrounded;
    private float fIFramesDuration = 2;
    private int iNumberOfFlashes = 5;
    private SpriteRenderer cSpriteRenderer;



    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        cSpriteRenderer = GetComponent<SpriteRenderer>();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // movement
        playerRb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * fSpeed, playerRb.linearVelocity.y);
        
        // jump
        if (Input.GetKeyDown("space") && bIsGrounded)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
        }

    }

   
    
    // grounding checks
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
    
    // Damage/I-Frames
    public void TakeDamage(int damage)
        {
            iPlayerHealth -= damage;
            
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
        
        // flashes
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
