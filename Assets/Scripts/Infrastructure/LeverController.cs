using UnityEngine;

namespace Infrastructure
{
    public class LeverController : MonoBehaviour
    {
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
