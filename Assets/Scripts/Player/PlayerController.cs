using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    
    public float FInput{ get; private set; }
    
    [SerializeField] private StatBlockUI statBlockUI;
    
    public PlayerMovement playerMovement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && Time.timeScale != 0)
        {
            playerMovement.Jump();
        }
        
    }

    public void FixedUpdate()
    {
        FInput = Input.GetAxisRaw("Horizontal");
    }

}

