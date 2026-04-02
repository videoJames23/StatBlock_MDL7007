using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats Preset", fileName = "NewEnemyStatsPreset")]
public class EnemyStatsPresetSO : ScriptableObject
{
    public int enemyHealth = 1;
    public float enemySpeed = 3f;
    public float enemySize  = 4.5f;
    public int enemyDir = 1;
    public int pointsTotalE;

    public EnemyRuntimeStats CreateRuntimeCopy()
    {
        return new EnemyRuntimeStats
        {
            enemyHealth = enemyHealth,
            enemySpeed  = enemySpeed,
            enemySize   = enemySize,
            enemyDir = enemyDir,
        };
    }
}