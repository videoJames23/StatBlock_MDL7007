using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Responsible for:
    // following player during tutorial level
    
    public GameObject player;
    [SerializeField] private int offsetVertical;
    
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
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetVertical, -10);
        }
        
    }
}
