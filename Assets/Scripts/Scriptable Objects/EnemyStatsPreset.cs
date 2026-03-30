using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats Preset", fileName = "NewEnemyStatsPreset")]
public class EnemyStatsPresetSO : ScriptableObject
{
    public int iEnemyHealth = 1;
    public float fEnemySpeed = 3f;
    public float fEnemySize  = 4.5f;
    public int iEnemyDir = 1;
    public int iPointsTotalE;

    public EnemyRuntimeStats CreateRuntimeCopy()
    {
        return new EnemyRuntimeStats
        {
            iEnemyHealth = iEnemyHealth,
            fEnemySpeed  = fEnemySpeed,
            fEnemySize   = fEnemySize,
            iEnemyDir = iEnemyDir,
        };
    }
}