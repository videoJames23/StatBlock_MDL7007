using UnityEngine;

[System.Serializable]

public class EnemyRuntimeStats
{
    public int iEnemyHealth;
    public float fEnemySpeed;
    public float fEnemySize;

    public EnemyRuntimeStats(EnemyStats baseStats)
    {
        iEnemyHealth = baseStats.iEnemyHealth;
        fEnemySpeed = baseStats.fEnemySpeed;
        fEnemySize = baseStats.fEnemySize;
    }
}