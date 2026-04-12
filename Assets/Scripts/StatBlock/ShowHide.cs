using UnityEngine;

public class ShowHide : MonoBehaviour
{
    // Responsible for:
    // showing or hiding UI text when instructed
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
