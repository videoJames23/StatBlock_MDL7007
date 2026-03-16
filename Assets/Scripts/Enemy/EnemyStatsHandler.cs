using UnityEngine;

public class EnemyStatsHandler : MonoBehaviour
{
    public EnemyStats baseStats;
    public EnemyRuntimeStats runtimeStats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloneStats();
    }

    // Update is called once per frame
    public void CloneStats()
    {
        runtimeStats = new EnemyRuntimeStats(baseStats);
        
    }
}