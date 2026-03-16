using System.Linq;
using UnityEngine;

public class StatBlockChanges : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    
    [SerializeField] private Rigidbody2D enemyRb;       
    [SerializeField] private Transform enemyVisual; 
    [SerializeField] private SpriteRenderer enemyRenderer; 

    
    public int[] statsP = {1, 1, 1};
    public int[] statsE = {1, 1, 1};
    
    private StatBlockInput statBlockInput;
    private StatBlockUI statBlockUI;
    
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    
    private EnemyController enemyController;
    private Transform enemyTransform;
    private EnemyStatsHandler enemyStatsHandler;
    
    public int iPointsTotalP;
    public int iPointsLeftP;
    public int iPointsTotalE;
    public int iPointsLeftE;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statBlockInput = GetComponent<StatBlockInput>();
        statBlockUI = GetComponent<StatBlockUI>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        
        GameObject enemyVisual = GameObject.FindGameObjectWithTag("EnemyVisual");
        if (enemyVisual)
        {
            enemyController = enemyVisual.GetComponent<EnemyController>();
            enemyTransform = enemyVisual.GetComponent<Transform>();
            enemyStatsHandler = enemyVisual.GetComponent<EnemyStatsHandler>();
        }
        GameObject enemyRoot = GameObject.FindGameObjectWithTag("EnemyRoot");
        if (enemyRoot)
        {
            enemyRb = enemyRoot.GetComponent<Rigidbody2D>();
        }
        
        
        iPointsLeftP = iPointsTotalP - statsP.Sum();
        iPointsLeftE = iPointsTotalE - statsE.Sum();
        
        StatChangePHealth();
        StatChangePSpeed();
        StatChangePJump();
        StatChangeEHealth();
        StatChangeESpeed();
        StatChangeESize();
        
    }
    
    private float GetSpriteHeightUnits(SpriteRenderer sr)
    {
        if (!sr || sr.sprite == null) return 1f;
        // This is in sprite local units (pixelsPerUnit), it does NOT depend on Transform scale.
        return sr.sprite.bounds.size.y;
    }

    
    public void StatChangePHealth()
    {
        playerStatsHandler.runtimeStats.iPlayerHealth = statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statsP[1]) // player speeds
            {
                
                case 0: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL0; break;
                case 1: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL1; break;
                case 2: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL2; break;
                case 3: playerStatsHandler.runtimeStats.fPlayerSpeed = playerStats.playerSpeedLVL3; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statsP[2]) //player jump heights
            {
                
                case 0: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL0; break;
                case 1: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL1; break;
                case 2: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL2; break;
                case 3: playerStatsHandler.runtimeStats.fPlayerJump = playerStats.playerJumpLVL3; break;
            }
        }
    }
    
    
    public void StatChangeEHealth()
    {
        if (!enemyRb || !enemyVisual) return;
        
        enemyStatsHandler.runtimeStats.iEnemyHealth = statsE[0];
        
    }
    public void StatChangeESpeed()
    {
            if (!enemyRb || !enemyVisual) return;

            float newSpeed = statsE[1] switch
            {
                0 => enemyStats.enemySpeedLVL0,
                1 => enemyStats.enemySpeedLVL1,
                2 => enemyStats.enemySpeedLVL2,
                3 => enemyStats.enemySpeedLVL3,
                _ => enemyRb.linearVelocity.x
            };

            enemyStatsHandler.runtimeStats.fEnemySpeed = newSpeed;
    }
    
    public void StatChangeESize()
    {
        if (!enemyRb || !enemyVisual) return;

        float newScale = statsE[2] switch
        {
            1 => enemyStats.enemySizeLVL1,
            2 => enemyStats.enemySizeLVL2,
            3 => enemyStats.enemySizeLVL3,
            _ => enemyVisual.localScale.x
        };

        enemyStatsHandler.runtimeStats.fEnemySize = newScale;
        ApplyEnemyScaleBottomAnchored(newScale);
    }

    
    private void ApplyEnemyScaleBottomAnchored(float scale)
    {
        Vector2 rootPosition = enemyRb.position;
        
        var localScale = enemyVisual.localScale;
        enemyVisual.localScale = new Vector3(scale, scale, localScale.z);
        
        float spriteHeight = GetSpriteHeightUnits(enemyRenderer);
        float childLocalY = (spriteHeight * scale) * 0.5f;
        var localPosition = enemyVisual.localPosition;
        enemyVisual.localPosition = new Vector3(localPosition.x, childLocalY, localPosition.z);
        
        enemyRb.position = rootPosition;
        
    }

}
