using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatValues  playerStats;
    
    public float Input{ get; private set; }
    
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

