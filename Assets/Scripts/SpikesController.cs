using UnityEngine;


public class SpikesController : MonoBehaviour
{
    public Rigidbody2D spikeRb;
    public float fSpikeSpeed;
    public float fSpikeDir;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spikeRb =  GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            spikeRb.linearVelocity = new Vector2(fSpikeSpeed * fSpikeDir, 0);
        }
    }
}
