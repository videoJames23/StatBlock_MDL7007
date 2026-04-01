using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StatBlockInput : MonoBehaviour
{
    private PlayerController playerController;
    
    [SerializeField] private GameManager gameManager;
    
    private StatBlockUI statBlockUI;

    private int maxIndex = 2;
    private int minIndex = 0;
    public delegate void IndexChanged();
    public static event IndexChanged OnIndexChanged;

    public delegate void StatIncreaseP(int selectedIndex);
    public static event StatIncreaseP OnStatIncreaseP;
    public delegate void StatDecreaseP(int selectedIndex);
    public static event StatDecreaseP OnStatDecreaseP;
    
    public delegate void StatIncreaseE(int selectedIndex);
    public static event StatIncreaseE OnStatIncreaseE;
    public delegate void StatDecreaseE(int selectedIndex);
    public static event StatDecreaseE OnStatDecreaseE;

    public int SelectedIndex{ get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        statBlockUI = GetComponent<StatBlockUI>();
        SelectedIndex = 0;
        
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
            SelectedIndex--;
            
            if (SelectedIndex < minIndex)
            {
                SelectedIndex = maxIndex;
            }
            
            OnIndexChanged?.Invoke();
            statBlockUI.UpdateUI();
        }


        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SelectedIndex++;
            
            if (SelectedIndex > maxIndex)
            {
                SelectedIndex = minIndex;
            }
            
            OnIndexChanged?.Invoke();
            statBlockUI.UpdateUI();
        }

        // change value
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu)
            {
                OnStatIncreaseP?.Invoke(SelectedIndex);
            }
            else if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu)
            {
                OnStatIncreaseE?.Invoke(SelectedIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.PlayerMenu)
            {
                OnStatDecreaseP?.Invoke(SelectedIndex);
            }
            else if (statBlockUI.CurrentMode == StatBlockUI.MenuMode.EnemyMenu)
            {
                OnStatDecreaseE?.Invoke(SelectedIndex);
            }
        }
    }
    
}
