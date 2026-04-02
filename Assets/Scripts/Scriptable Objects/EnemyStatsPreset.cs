using UnityEngine;

[CreateAssetMenu(menuName = "Starting Stats/Enemy Stats Preset", fileName = "NewEnemyStatsPreset")]
public class EnemyStatsPresetSO : ScriptableObject
{
    [Header("Starting Stat Levels")]
    public StatLevel healthLevel = StatLevel.Level1;
    public StatLevel speedLevel  = StatLevel.Level1;
    public StatLevel sizeLevel   = StatLevel.Level1;

    // +1 == right, -1 == left
    [Header ("Starting Direction")]
    public int enemyDir = 1;
    
    // Total points for the enemy in this level
    [Header("Points")]
    public int pointsTotal = 5;
    
}