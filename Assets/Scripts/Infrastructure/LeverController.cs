using UnityEngine;

namespace Infrastructure
{
    public class LeverController : MonoBehaviour
    {
        // Responsible for:
        // detecting player/enemy collision
        // &
        // disabling gate
        
        public GameObject gate;

        private void Start()
        {
            gate =  GameObject.FindGameObjectWithTag("Gate");
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("EnemyVisual"))
            {
                gate.SetActive(false);
            }
        }
    }
}
