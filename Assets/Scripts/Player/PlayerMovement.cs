using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 12f;
    public float airControlMultiplier = 0.5f;
    
    private Rigidbody2D playerRb;
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    [SerializeField] PlayerStats playerStats;
    
    public bool bIsGrounded;

    public delegate void JumpEvent();
    public static event JumpEvent OnJumpEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
       
    }
    
    public void Jump()
    {
        if (bIsGrounded)
        {
            OnJumpEvent?.Invoke();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerStatsHandler.runtimeStats.fPlayerJump);
            bIsGrounded = false;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vTargetVelocity = new Vector2(playerController.fInput * playerStatsHandler.runtimeStats.fPlayerSpeed, playerRb.linearVelocity.y);

        float fControl = bIsGrounded ? 1f : airControlMultiplier;

        playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, vTargetVelocity,acceleration * fControl * Time.fixedDeltaTime);
    }
}
