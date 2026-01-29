using UnityEngine;

public class CameraController : MonoBehaviour
{
    //fred here, adding this comment to test version control
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player =  GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, -10);
        }
        
    }
}
