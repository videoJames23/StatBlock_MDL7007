using UnityEngine;

namespace Infrastructure
{
    public class SpikesController : MonoBehaviour
    {
        public Rigidbody2D spikeRb;
        [SerializeField] private float spikeSpeed;
        [SerializeField] private float spikeDir;
        private bool entityDetected;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            spikeRb =  GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (entityDetected)
            {
                spikeRb.linearVelocity = new Vector2(spikeSpeed * spikeDir, 0);
            }
                
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("EnemyVisual"))
            {
                entityDetected = true;
                spikeRb.linearVelocity = new Vector2(spikeSpeed * spikeDir, 0);
            }
        }
    }
}
