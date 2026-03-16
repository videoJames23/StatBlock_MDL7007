using UnityEngine;

[System.Serializable]

public class PlayerRuntimeStats
{
    public int iPlayerHealth;
    public float fPlayerSpeed;
    public float fPlayerJump;

    public PlayerRuntimeStats(PlayerStats baseStats)
    {
        iPlayerHealth = baseStats.iPlayerHealth;
        fPlayerSpeed = baseStats.fPlayerSpeed;
        fPlayerJump = baseStats.fPlayerJump;
    }
}
