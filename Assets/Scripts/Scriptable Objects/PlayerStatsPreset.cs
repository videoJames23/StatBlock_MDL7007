using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player Stats Preset", fileName = "NewPlayerStatsPreset")]
public class PlayerStatsPresetSO : ScriptableObject
{
    public int playerHealth = 1;
    public float playerSpeed = 3f;
    public float playerJump  = 5f;
    public int pointsTotalP = 6;

    public PlayerRuntimeStats CreateRuntimeCopy()
    {
        return new PlayerRuntimeStats
        {
            playerHealth = playerHealth,
            playerSpeed  = playerSpeed,
            playerJump   = playerJump,
        };
    }
}
