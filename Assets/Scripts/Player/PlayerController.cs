using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    
    
    [FormerlySerializedAs("fSpeed")] [Header("Stats")]
    
    [SerializeField] private PlayerStats  playerStats;
    
   
    public bool bInMenu;
    public bool bInMenuP;
    public bool bInMenuE;
    
    
    

    public float fInput;
    
    
    
    private StatBlockUI statBlockUI;
    private GameManager gameManagerScript;
    
    public PlayerMovement playerMovement;
    public PlayerDamage playerDamage;


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerRb = GetComponent<Rigidbody2D>();
        playerDamage = GetComponent<PlayerDamage>();
        playerMovement = GetComponent<PlayerMovement>();
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        
        
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !bInMenu)
        {
            playerMovement.Jump();
        }
        
    }

    public void FixedUpdate()
    {

        if (!bInMenu)
        {
            fInput = Input.GetAxisRaw("Horizontal");
        }
    }

}

