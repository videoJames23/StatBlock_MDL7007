using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    private bool bCanTakeDamage = true;
    private float fIFramesDuration = 1;
    private int iNumberOfFlashes = 5;
    private SpriteRenderer cSpriteRenderer;
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRb;
    private AudioController audioController;
    private GameManager gameManagerScript;
    private StatBlockUI statBlockUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        cSpriteRenderer = GetComponent<SpriteRenderer>(); 
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        GameObject statBlockUI = GameObject.FindGameObjectWithTag("StatBlockUI");
        this.statBlockUI = statBlockUI.GetComponent<StatBlockUI>();
        
        
        GameObject audio =  GameObject.FindGameObjectWithTag("Audio");
        if (audio)
        {
            audioController = audio.GetComponent<AudioController>();
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    
    public void TakeDamage(int iDamage)
    {
        if (!bCanTakeDamage)
        {
            return;
        }
        
        bCanTakeDamage = false;
        
        audioController.damageSource.Play();
        
        statBlockUI.statsP[0] -= iDamage;
        statBlockUI.iPointsTotalP--;
            
        gameManagerScript.StatChangePHealth();
        playerController.bInMenuP = true;
        statBlockUI.UpdateUI();
        playerController.bInMenuP = false;
        statBlockUI.UpdateUI();
            
            
        // I-frames
        if (playerStats.iPlayerHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
            
        else if (playerStats.iPlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        
      
        for (int i = 0; i < iNumberOfFlashes; i++)
        {
            cSpriteRenderer.color = new Color(0, 0.25f, 1, 0.5f);
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
            cSpriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(fIFramesDuration/iNumberOfFlashes);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
        bCanTakeDamage = true;
    }
}
