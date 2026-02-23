using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int iEnemyHealth;
    
    public float fEnemySpeed;
    
    public float fEnemySize;
    
    public int[,] aEnemyStatBounds = { {1, 3}, {0, 3}, {1, 3} };
    
    // fEnemyDir controls whether the enemy is moving left or right, but you've used a float even though it can only be -1 or 1, maybe another type would work better.
    
    
    public int iDamage;
    public float fCosAngle = 0.70710678118f;
}
