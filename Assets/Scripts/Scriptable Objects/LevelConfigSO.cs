// LevelConfigSO.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Config", fileName = "NewLevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Player")]
    public PlayerStatsPresetSO playerStartingPreset;
}
