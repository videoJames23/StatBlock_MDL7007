using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        public GameObject levelSelectUI;
        public GameObject creditsUI;
        
        public AudioController audioController;
        
        
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
            Button buttonStart = root.Q<Button>("start__button");
            Button buttonLevelSelect = root.Q<Button>("level__select__button");
            Button buttonCredits = root.Q<Button>("credits__button");
            
            GameObject audio = GameObject.FindGameObjectWithTag("Audio");
            if (audio)
            {
                audioController = audio.GetComponent<AudioController>();
            }
            
           
            
            buttonStart.clicked += () => SceneManager.LoadScene(1);
            
            buttonLevelSelect.clicked += () => Destroy(gameObject);
            buttonLevelSelect.clicked += () => Instantiate(levelSelectUI);
            buttonLevelSelect.clicked += () => audioController.upSource.Play();
            
            buttonCredits.clicked += () => Destroy(gameObject);
            buttonCredits.clicked += () => Instantiate(creditsUI);
            buttonCredits.clicked += () => audioController.upSource.Play();
        }

    }
}
