using UnityEngine;

public class ShowHide : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // produces nullReferenceException
        if (gameObject.CompareTag("Jump"))
        {
            Show();
        }
        else if (gameObject.CompareTag("Size"))
        {
            Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
