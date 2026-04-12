using UnityEngine;

public class Music : MonoBehaviour
{
    // Responsible for:
    // Ensuring music plays uninterrupted between levels
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
