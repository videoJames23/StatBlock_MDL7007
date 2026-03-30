using Unity.VisualScripting;
using UnityEngine;

public class StatBlockInput : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private EnemyStats enemyStats;
    
    [SerializeField] private GameManager gameManager;
    
    private StatBlockUI statBlockUI;
    
    
    public delegate void Index();
    public static event Index OnIndex;

    public delegate void StatIncreaseP(int selectedIndex);
    public static event StatIncreaseP OnStatIncreaseP;
    public delegate void StatDecreaseP(int selectedIndex);
    public static event StatDecreaseP OnStatDecreaseP;
    
    public delegate void StatIncreaseE(int selectedIndex);
    public static event StatIncreaseE OnStatIncreaseE;
    public delegate void StatDecreaseE(int selectedIndex);
    public static event StatDecreaseE OnStatDecreaseE;
    
    public int selectedIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        statBlockUI = GetComponent<StatBlockUI>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController) return;
        if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.None ||
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerPreview ||
            statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyPreview) return;
        
        // select stat
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex--;
            OnIndex?.Invoke();

            switch (selectedIndex)
            {
                case -1: selectedIndex = 2; break;
                case 0:
                case 1:
                case 2: break;
            }
            statBlockUI.UpdateUI();
        }


        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex++;
            OnIndex?.Invoke();

            switch (selectedIndex)
            {
                case 0:
                case 1:
                case 2: break;
                case 3: selectedIndex = 0; break;
            }
            statBlockUI.UpdateUI();
        }

        // change value
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu)
            {
                OnStatIncreaseP?.Invoke(selectedIndex);
            }
            else if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu)
            {
                OnStatIncreaseE?.Invoke(selectedIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu)
            {
                OnStatDecreaseP?.Invoke(selectedIndex);
            }
            else if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu)
            {
                OnStatDecreaseE?.Invoke(selectedIndex);
            }
        }
    }
    
}
