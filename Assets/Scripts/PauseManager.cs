using Player;
using StatBlock;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private StatBlockUI statBlockUI;
    [SerializeField] private PlayerCollisions playerCollisions;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (playerCollisions.IsTouchingStatBlockP)
            {
                statBlockUI.TogglePlayerMenu();
            }

            else if (playerCollisions.IsTouchingStatBlockE)
            {
                statBlockUI.ToggleEnemyMenu();
            }
            
            UpdatePauseState();
        }
    }
    
    private void UpdatePauseState()
    {
        var shouldPause =
            statBlockUI.CurrentMode is StatBlockUI.MenuMode.PlayerMenu or StatBlockUI.MenuMode.EnemyMenu;

        Time.timeScale = shouldPause ? 0 : 1;
    }
}
