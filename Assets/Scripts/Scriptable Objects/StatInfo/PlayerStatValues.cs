using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatValues", menuName = "Stat Values/Player Stat Values")]
public class PlayerStatValues : ScriptableObject
{
   // Actual values for stats by level
   public int[] healthByLevel  = { 0, 1, 2, 3 };
   public float[] speedByLevel = { 0f, 3f, 7f, 10f };
   public float[] jumpByLevel  = { 0f, 5f, 7f, 9f };
   
   // Minimum, maximum values for each stat that can be assigned via StatBlock (Health, speed, jump)
   public int[,] playerStatBounds = { {1, 3}, {0, 3}, {0, 3} };
   
}
