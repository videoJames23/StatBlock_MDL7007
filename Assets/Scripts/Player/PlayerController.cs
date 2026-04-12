using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Responsible for:
        // gathering player input
        // &
        // exposing input for playerMovement
        // &
        // instructing playerMovement to jump
        
        [SerializeField] private PlayerMovement playerMovement;
        public float Input{ get; private set; }

        // Update is called once per frame
        private void Update()
        {
            if ((UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.Space)) && Time.timeScale != 0)
            {
                playerMovement.Jump();
            }
        }

        public void FixedUpdate()
        {
            Input = UnityEngine.Input.GetAxisRaw("Horizontal");
        }
    }
}

