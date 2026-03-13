using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{

    public GameObject mainMenuUI;
    public delegate void Back();

    public static event Back OnBack;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        Button buttonBack = root.Q<Button>("back__button");
        Button buttonLevelT1 = root.Q<Button>("tutorial__button");
        Button buttonLevelT2 = root.Q<Button>("tutorial__enemy__button");
        Button buttonLevel1 = root.Q<Button>("level__1__button");
        Button buttonLevel2 = root.Q<Button>("level__2__button");
        Button buttonLevel3 = root.Q<Button>("level__3__button");
        Button buttonLevel4 = root.Q<Button>("level__4__button");
        

        buttonBack.clicked += () => Destroy(gameObject);
        buttonBack.clicked += () => Instantiate(mainMenuUI);
        buttonBack.clicked += () => OnBack?.Invoke();
        
        buttonLevelT1.clicked += () => SceneManager.LoadScene(1);
        
        buttonLevelT2.clicked += () => SceneManager.LoadScene(2);
        
        buttonLevel1.clicked += () => SceneManager.LoadScene(3);
        
        buttonLevel2.clicked += () => SceneManager.LoadScene(4);
        
        buttonLevel3.clicked += () => SceneManager.LoadScene(5);
        
        buttonLevel4.clicked += () => SceneManager.LoadScene(6);
        
        
    }
    
}
