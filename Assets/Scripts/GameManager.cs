using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCollisions  playerCollisions;
    
    [SerializeField] private StatBlockUI statBlockUI;
    
    private int iBuildIndex;
    
    private void OnEnable()
    {
        PlayerCollisions.OnCompletion += OnCompletion;
    }

    private void OnDisable()
    {
        PlayerCollisions.OnCompletion -= OnCompletion;
    }
    
    void Start()
    {
        iBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(iBuildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (playerCollisions.BIsTouchingStatBlockP)
            {
                statBlockUI.TogglePlayerMenu();
            }

            else if (playerCollisions.BIsTouchingStatBlockE)
            {
                statBlockUI.ToggleEnemyMenu();
            }

            else
            {
                statBlockUI.SetMenuMode(StatBlockUI.MenuMode.None);
            }

            PauseChecks();

        }
    }
    
    private void PauseChecks()
    {
        bool shouldPause =
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu ||
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu;

        Time.timeScale = shouldPause ? 0 : 1;
    }
    
    private void OnCompletion()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(iBuildIndex + 1);
    }



}
