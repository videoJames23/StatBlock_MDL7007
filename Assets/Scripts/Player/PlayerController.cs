using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    
    [SerializeField] private PlayerStats  playerStats;
    
    
    public float fInput;
    
    
    
    private GameManager gameManager;
    
    public PlayerMovement playerMovement;
    

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerMovement = GetComponent<PlayerMovement>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        this.gameManager = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !gameManager.BInMenu)
        {
            playerMovement.Jump();
        }
        
    }

    public void FixedUpdate()
    {
        if (!gameManager.BInMenu)
        {
            fInput = Input.GetAxisRaw("Horizontal");
        }
    }

}

