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
        bool shouldPause =
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu ||
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu;

        Time.timeScale = shouldPause ? 0 : 1;
    }
}
