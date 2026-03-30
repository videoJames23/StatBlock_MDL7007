using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 12f;
    public float airControlMultiplier = 0.5f;
    
    private Rigidbody2D playerRb;
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    [SerializeField] PlayerStats playerStats;

    private bool bIsGrounded;

    public delegate void JumpEvent();
    public static event JumpEvent OnJumpEvent;

    private void OnEnable()
    {
        EnemyCollisions.OnPlayerSquash += Ground;
        PlayerCollisions.OnGround += Ground;
        PlayerCollisions.OnUnGround += UnGround;
        EnemyCollisions.OnPlayerSquash += Jump;
    }

    private void OnDisable()
    {
        EnemyCollisions.OnPlayerSquash -= Ground;
        PlayerCollisions.OnGround -= Ground;
        PlayerCollisions.OnUnGround -= UnGround;
        EnemyCollisions.OnPlayerSquash -= Jump;
    }
    
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

    private void Ground()
    {
        bIsGrounded = true;
    }

    private void UnGround()
    {
        bIsGrounded = false;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vTargetVelocity = new Vector2(playerController.FInput * playerStatsHandler.runtimeStats.fPlayerSpeed, playerRb.linearVelocity.y);

        float fControl = bIsGrounded ? 1f : airControlMultiplier;

        playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, vTargetVelocity,acceleration * fControl * Time.fixedDeltaTime);
    }
}
