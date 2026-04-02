using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
   
    public int[,] enemyStatBounds = { {1, 3}, {0, 3}, {1, 3} };
    
    [SerializeField] public float enemySpeedLVL0 = 0f;
    [SerializeField] public float enemySpeedLVL1 = 3f;
    [SerializeField] public float enemySpeedLVL2 = 7f;
    [SerializeField] public float enemySpeedLVL3 = 10f;
    
    [SerializeField] public float enemySizeLVL1 = 1.5f;
    [SerializeField] public float enemySizeLVL2 = 3f;
    [SerializeField] public float enemySizeLVL3 = 4.5f;
    
    public float cosAngle = 0.70710678118f;
}
