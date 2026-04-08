using Enemy;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerStatValues playerStats;
        public float acceleration = 12f;
        public float airControlMultiplier = 0.5f;
    
        private Rigidbody2D playerRb;
        private PlayerController playerController;
        private PlayerStatsHandler playerStatsHandler;
        
        private bool isGrounded;

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
        private void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();
            playerController = GetComponent<PlayerController>();
            playerStatsHandler = GetComponent<PlayerStatsHandler>();
       
        }
    
        public void Jump()
        {
            if (!isGrounded) return;
            OnJumpEvent?.Invoke();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerStatsHandler.runtimeStats.playerJump);
            isGrounded = false;
        }

        private void Ground()
        {
            isGrounded = true;
        }

        private void UnGround()
        {
            isGrounded = false;
        }
    
        // Update is called once per frame
        private void FixedUpdate()
        {
            var targetVelocity = new Vector2(playerController.Input * playerStatsHandler.runtimeStats.playerSpeed, playerRb.linearVelocity.y);

            var control = isGrounded ? 1f : airControlMultiplier;

            playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, targetVelocity,acceleration * control * Time.fixedDeltaTime);
        }
    }
}
