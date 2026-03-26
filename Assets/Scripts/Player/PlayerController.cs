using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    
    
    [Header("Stats")]
    
    [SerializeField] private PlayerStats  playerStats;
    
    
    public float fInput;
    
    
    
    private StatBlockUI statBlockUI;
    private GameManager gameManager;
    
    public PlayerMovement playerMovement;
    public PlayerDamage playerDamage;


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        playerDamage = GetComponent<PlayerDamage>();
        playerMovement = GetComponent<PlayerMovement>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        this.gameManager = gameManager.GetComponent<GameManager>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
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

