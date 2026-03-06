using System.Linq;
using UnityEngine;

public class StatBlockChanges : MonoBehaviour
{
    [SerializeField] private PlayerStats  playerStats;
    [SerializeField] private EnemyStats  enemyStats;
    
    public int[] statsP = {1, 1, 1};
    public int[] statsE = {1, 1, 1};
    
    private StatBlockInput statBlockInput;
    private StatBlockUI statBlockUI;
    
    private PlayerController playerController;
    private EnemyController enemyController;
    private Rigidbody2D enemyRb;
    
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
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy)
        {
            enemyController = enemy.GetComponent<EnemyController>();
            enemyRb = enemy.GetComponent<Rigidbody2D>();
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
    public void StatChangePHealth()
    {
        playerStats.iPlayerHealth = statsP[0];
    }
    public void StatChangePSpeed()
    {
        if (playerController)
        {
            switch (statsP[1]) // player speeds
            {
                
                case 0: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL0; break;
                case 1: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL1; break;
                case 2: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL2; break;
                case 3: playerStats.fPlayerSpeed = playerStats.playerSpeedLVL3; break;
            }
        }
    }
    public void StatChangePJump()
    {
        if (playerController)
        {
            switch (statsP[2]) //player jump heights
            {
                
                case 0: playerStats.fPlayerJump = playerStats.playerJumpLVL0; break;
                case 1: playerStats.fPlayerJump = playerStats.playerJumpLVL1; break;
                case 2: playerStats.fPlayerJump = playerStats.playerJumpLVL2; break;
                case 3: playerStats.fPlayerJump = playerStats.playerJumpLVL3; break;
            }
        }
    }
    
    
    public void StatChangeEHealth()
    {
        if (enemyController)
        {
            enemyStats.iEnemyHealth = statsE[0];
        }
    }
    public void StatChangeESpeed()
    {
        if (enemyController)
        {
            switch (statsE[1]) // enemy speeds
            {
                case 0: enemyStats.fEnemySpeed = enemyStats.enemySpeedLVL0; break;
                case 1: enemyStats.fEnemySpeed = enemyStats.enemySpeedLVL1; break;
                case 2: enemyStats.fEnemySpeed = enemyStats.enemySpeedLVL2; break;
                case 3: enemyStats.fEnemySpeed = enemyStats.enemySpeedLVL3; break;
            }
        }
    }
    public void StatChangeESize()
    {
        if (enemyController)
        {
            switch (statsE[2]) //enemy sizes
            {

                // If enemy grows into wall, movement stops
                
                //I'm assuming it's dependent on the differences between sizes, but the Y offset floats are more magic numbers,
                //which could be replaced with a calculation which works regardless of what you set the sizes to -F

                case 1:
                    enemyStats.fEnemySize = enemyStats.enemySizeLVL1;
                    if (statBlockUI.iPrevSize != 1)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.81f);
                    }

                    break;
                case 2:
                    
                    switch (statBlockUI.iPrevSize)
                    {
                        case 1:
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.81f);
                            enemyStats.fEnemySize = enemyStats.enemySizeLVL2;
                            break;
                        case 3:
                            enemyStats.fEnemySize = enemyStats.enemySizeLVL2;
                            enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y - 0.726443f);
                            break;
                    }

                    break;
                case 3:
                    if (statBlockUI.iPrevSize != 3)
                    {
                        enemyRb.position = new Vector2(enemyRb.position.x, enemyRb.position.y + 0.726443f);
                        enemyStats.fEnemySize = enemyStats.enemySizeLVL3;
                    }

                    break;
            }
            enemyRb.transform.localScale = new Vector2(enemyStats.fEnemySize, enemyStats.fEnemySize);
        }
    }
}
