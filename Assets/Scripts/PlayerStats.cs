using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
   public int iPlayerHealth = 3;
   
   public float fPlayerSpeed = 3;
   
   public float fPlayerJump = 3;

   public int[,] aPlayerStatBounds = { {1, 3}, {0, 3}, {0, 3} };
   
   public Vector2 vPlayerVelocity;
   
   public float playerSpeedLVL0 = 0f;
   public float playerSpeedLVL1 = 3f;
   public float playerSpeedLVL2 = 7f;
   public float playerSpeedLVL3 = 10f;
    
   public float playerJumpLVL0 = 0f;
   public float playerJumpLVL1 = 5f;
   public float playerJumpLVL2 = 7f;
   public float playerJumpLVL3 = 9f;
}
