using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    public bool bIsGrounded;
    public float fSpeed = 10;
    public float fJumpForce = 7.5f;
    public int iPlayerHealth = 3;
    private float fIFramesDuration = 2;
    private int iNumberOfFlashes = 3;
    private SpriteRenderer spriteRenderer;



    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        playerRb.linearVelocity =
            new Vector2(Input.GetAxis("Horizontal") * fSpeed,
                playerRb.linearVelocity.y); // get horizontal movement info

        if (Input.GetKeyDown("space") && bIsGrounded) // Jump
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, fJumpForce);
        }

    }

    public void TakeDamage(int damage)
    {
        iPlayerHealth -= damage;
        
        if (iPlayerHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }

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

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(0, 0, 1, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            spriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
