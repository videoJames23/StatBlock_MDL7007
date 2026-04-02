using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Config", fileName = "NewLevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Player")]
    public PlayerStatsPresetSO playerStartingPreset;

    [Header("Enemy")] 
    public EnemyStatsPresetSO enemyStartingPreset;
    
    
    [Header("StatBlock UI (preview layout)")]
    public Vector2 uiHolderAnchoredPosition = new (-601.17f, -373.82f);
    public Vector3 uiHolderScale            = new (1f, 1f, 1f);
    public Vector3 uiBackgroundScale        = new (2.83f, 2.31f, 1f);

    
}
