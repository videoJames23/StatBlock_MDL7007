using UnityEngine;

namespace Scriptable_Objects.StatInfo
{
    [CreateAssetMenu(fileName = "EnemyStatValues", menuName = "Stat Values/Enemy Stat Values")]
    public class EnemyStatValues : ScriptableObject
    {
        // Responsible for:
        // holding the actual values that correspond to each stat level
        // &
        // holding max/min values for each stat
        // &
        // holding angle of player/enemy intersection to evaluate collisions
        
        // Actual values for stats by level
        public int[] healthByLevel  = { 0, 1, 2, 3 };
        public float[] speedByLevel = { 0f, 3f, 7f, 10f };
        public float[] sizeByLevel  = { 0, 1.5f, 3f, 4.5f };
   
        // Minimum, maximum values for each stat that can be assigned via StatBlock (Health, speed, size)
        public readonly int[,] EnemyStatBounds = { {1, 3}, {0, 3}, {1, 3} };
    
        // Angle of intersection above which player landing on enemy results in an attack
        public float cosAngle = 0.70710678118f;
    }
}
