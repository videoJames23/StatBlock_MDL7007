using UnityEngine;

[CreateAssetMenu(menuName = "Starting Stats/Player Stats Preset", fileName = "NewPlayerStatsPreset")]
public class PlayerStatsPresetSO : ScriptableObject
{
    
    [Header("Starting Stat Levels")]
    public StatLevel healthLevel = StatLevel.Level1;
    public StatLevel speedLevel  = StatLevel.Level1;
    public StatLevel jumpLevel   = StatLevel.Level1;

    // Total points for the player in this level
    [Header("Points")]
    public int pointsTotal = 5;
    
}
