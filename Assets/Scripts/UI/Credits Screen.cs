using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsScreen : MonoBehaviour
{
    public GameObject mainMenuUI;
    private AudioSource downSource;

    public delegate void Back();
    public static event Back OnBack;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        Button buttonBack = root.Q<Button>("back__button");
        
        buttonBack.clicked += () => Destroy(gameObject);
        buttonBack.clicked += () => Instantiate(mainMenuUI);
        buttonBack.clicked += () => OnBack?.Invoke();

    }
}
