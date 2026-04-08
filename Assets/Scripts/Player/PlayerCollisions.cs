using UnityEngine;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private PlayerDamage playerDamage;
    
    
        public bool IsTouchingStatBlockP{ get; private set; }
        public bool IsTouchingStatBlockE{ get; private set; }

        public delegate void Completion();
        public static event Completion OnCompletion;

        public delegate void Ground();
        public static event Ground OnGround;
        public delegate void UnGround();
        public static event UnGround OnUnGround;

        private void Start()
        {
            playerDamage = GetComponent<PlayerDamage>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Spike"))
            {
                playerDamage.TakeDamage();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {

            switch (other.gameObject.tag)
            {  
                case "Finish":
                    Debug.Log("Level Complete!");
                    OnCompletion?.Invoke();
                    break;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                OnGround?.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "StatBlockP":
                    IsTouchingStatBlockP = true;
                    break;
                case "StatBlockE":
                    IsTouchingStatBlockE = true;
                    break;
            }
        }


        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                OnUnGround?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "StatBlockP":
                    IsTouchingStatBlockP = false;
                    break;
                case "StatBlockE":
                    IsTouchingStatBlockE = false;
                    break;
            }
        }
    }
}
