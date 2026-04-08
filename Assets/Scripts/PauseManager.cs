using StatBlock;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private StatBlockUI statBlockUI;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    
    public void UpdatePauseState()
    {
        var shouldPause =
            statBlockUI.CurrentMode is StatBlockUI.MenuMode.PlayerMenu or StatBlockUI.MenuMode.EnemyMenu;

        Time.timeScale = shouldPause ? 0 : 1;
    }
}
