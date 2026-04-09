using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatBlock
{
    public class StatBlockInput : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private StatBlockUI statBlockUI;

        private const int maxIndex = 2;
        private const int minIndex = 0;

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
        private void Start()
        { 
            SelectedIndex = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!playerController) return;
            if (statBlockUI.CurrentMode is StatBlockUI.MenuMode.None or StatBlockUI.MenuMode.PlayerPreview or StatBlockUI.MenuMode.EnemyPreview) return;
        
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
                switch (statBlockUI.CurrentMode)
                {
                    case StatBlockUI.MenuMode.PlayerMenu:
                        OnStatIncreaseP?.Invoke(SelectedIndex);
                        break;
                    case StatBlockUI.MenuMode.EnemyMenu:
                        OnStatIncreaseE?.Invoke(SelectedIndex);
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                switch (statBlockUI.CurrentMode)
                {
                    case StatBlockUI.MenuMode.PlayerMenu:
                        OnStatDecreaseP?.Invoke(SelectedIndex);
                        break;
                    case StatBlockUI.MenuMode.EnemyMenu:
                        OnStatDecreaseE?.Invoke(SelectedIndex);
                        break;
                }
            }
        }
    
    }
}
