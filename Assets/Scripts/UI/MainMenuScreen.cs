using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        public GameObject levelSelectUI;
        public GameObject creditsUI;

        private AudioSource upSource;
        
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
            Button buttonStart = root.Q<Button>("start__button");
            Button buttonLevelSelect = root.Q<Button>("level__select__button");
            Button buttonCredits = root.Q<Button>("credits__button");
            
            GameObject upAudio = GameObject.Find("Up");
            if (upAudio)
            {
                upSource = upAudio.GetComponent<AudioSource>();
            }
            
            buttonStart.clicked += () => SceneManager.LoadScene(1);
            
            buttonLevelSelect.clicked += () => Destroy(gameObject);
            buttonLevelSelect.clicked += () => Instantiate(levelSelectUI);
            buttonLevelSelect.clicked += () => upSource.Play();
            
            buttonCredits.clicked += () => Destroy(gameObject);
            buttonCredits.clicked += () => Instantiate(creditsUI);
            buttonCredits.clicked += () => upSource.Play();
        }

    }
}
