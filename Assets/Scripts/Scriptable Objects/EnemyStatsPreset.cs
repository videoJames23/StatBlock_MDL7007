using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats Preset", fileName = "NewEnemyStatsPreset")]
public class EnemyStatsPresetSO : ScriptableObject
{
    public int iEnemyHealth = 1;
    public float fEnemySpeed = 3f;
    public float fEnemySize  = 5f;

    public EnemyRuntimeStats CreateRuntimeCopy()
    {
        return new EnemyRuntimeStats
        {
            iEnemyHealth = this.iEnemyHealth,
            fEnemySpeed  = this.fEnemySpeed,
            fEnemySize   = this.fEnemySize
        };
    }
}