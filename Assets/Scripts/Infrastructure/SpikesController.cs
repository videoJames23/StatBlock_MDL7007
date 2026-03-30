using UnityEngine;


public class SpikesController : MonoBehaviour
{
    public Rigidbody2D spikeRb;
    [SerializeField] private float fSpikeSpeed;
    [SerializeField] private float fSpikeDir;
    private bool bEntityDetected;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spikeRb =  GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bEntityDetected)
        {
            spikeRb.linearVelocity = new Vector2(fSpikeSpeed * fSpikeDir, 0);
        }
                
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("EnemyVisual"))
        {
            bEntityDetected = true;
            spikeRb.linearVelocity = new Vector2(fSpikeSpeed * fSpikeDir, 0);
        }
    }
}
