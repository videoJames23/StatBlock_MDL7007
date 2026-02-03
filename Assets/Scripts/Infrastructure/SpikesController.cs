using UnityEngine;


public class SpikesController : MonoBehaviour
{
    public Rigidbody2D spikeRb;
    public float fSpikeSpeed;
    public float fSpikeDir;
    public PlayerController playerController;
    private bool bEntityDetected = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spikeRb =  GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            //this should all be replaced with a proper pause menu system -F
            if (playerController.bInMenu)
            {
                spikeRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else if //(!playerController.bInMenu) this is an extra condition check for no reason you're already using else -F
            {
                spikeRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                if (bEntityDetected)
                {
                    spikeRb.linearVelocity = new Vector2(fSpikeSpeed * fSpikeDir, 0);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            bEntityDetected = true;
            spikeRb.linearVelocity = new Vector2(fSpikeSpeed * fSpikeDir, 0);
        }
    }
}
