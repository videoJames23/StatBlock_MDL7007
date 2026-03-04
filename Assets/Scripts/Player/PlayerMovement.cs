using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 12f;
    public float airControlMultiplier = 0.5f;
    
    private Rigidbody2D playerRb;
    
    private PlayerController playerController;
    
    [SerializeField] PlayerStats playerStats;
    
    public bool bIsGrounded;
    
    private AudioController audioController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        GameObject audio =  GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController = audio.GetComponent<AudioController>();
        }
    }
    
    public void Jump()
    {
        if (bIsGrounded)
        {
            audioController.jumpSource.Play();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerStats.fPlayerJump);
            bIsGrounded = false;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vTargetVelocity = new Vector2(playerController.fInput * playerStats.fPlayerSpeed, playerRb.linearVelocity.y);

        float fControl = bIsGrounded ? 1f : airControlMultiplier;

        playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, vTargetVelocity,acceleration * fControl * Time.fixedDeltaTime);
    }
}
