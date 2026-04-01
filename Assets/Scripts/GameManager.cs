using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCollisions  playerCollisions;
    
    [SerializeField] private StatBlockUI statBlockUI;
    [SerializeField] private PauseManager pauseManager;
    
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

            pauseManager.UpdatePauseState();

        }
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
