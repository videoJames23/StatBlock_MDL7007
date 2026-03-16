using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{
    public PlayerStats baseStats;
    public PlayerRuntimeStats runtimeStats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloneStats();
    }

    // Update is called once per frame
    public void CloneStats()
    {
        runtimeStats = new PlayerRuntimeStats(baseStats);
    }
}
