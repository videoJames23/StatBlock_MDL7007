using System.Collections;
using Player;
using StatBlock;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private PlayerCollisions  playerCollisions;
    
    [SerializeField] private StatBlockUI statBlockUI;
    [SerializeField] private PauseManager pauseManager;
    
    private int buildIndex;
    
    private void OnEnable()
    {
        PlayerCollisions.OnCompletion += OnCompletion;
    }

    private void OnDisable()
    {
        PlayerCollisions.OnCompletion -= OnCompletion;
    }
    
    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(buildIndex);
        }
    }
    
    private void OnCompletion()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(buildIndex + 1);
    }
}
