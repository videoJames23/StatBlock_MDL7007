using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player Stats Preset", fileName = "NewPlayerStatsPreset")]
public class PlayerStatsPresetSO : ScriptableObject
{
    public int iPlayerHealth = 1;
    public float fPlayerSpeed = 3f;
    public float fPlayerJump  = 5f;
    public int iPointsTotalP = 6;

    public PlayerRuntimeStats CreateRuntimeCopy()
    {
        return new PlayerRuntimeStats
        {
            iPlayerHealth = this.iPlayerHealth,
            fPlayerSpeed  = this.fPlayerSpeed,
            fPlayerJump   = this.fPlayerJump,
            iPointsTotalP = this.iPointsTotalP,
        };
    }
}
