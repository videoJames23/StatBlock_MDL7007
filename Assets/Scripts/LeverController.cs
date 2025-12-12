using UnityEngine;

public class LeverController : MonoBehaviour
{

    public GameObject gate;
   
    void Start()
    {
        gate =  GameObject.FindGameObjectWithTag("Gate");
    }

    

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            gate.SetActive(false);
        }
    }
}
