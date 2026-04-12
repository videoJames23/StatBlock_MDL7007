using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        // Responsible for:
        // determining whether enemy should take damage
        // &
        // informing StatBlockChangesE of damage taken
        // &
        // setting and disabling enemy invulnerability
        // &
        // flashing enemy sprite during invulnerability
        
        [SerializeField] private EnemyStatsHandler enemyStatsHandler;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        
        private const float iFramesDuration = 1; // invulnerability duration in seconds
        private const int numberOfFlashes = 5; // number of sprite flashes to indicate invulnerability
        public bool CanTakeDamage{ get; private set; }

        public delegate void Damage();
        public static event Damage OnDamage;

        private void OnEnable()
        {
            EnemyCollisions.OnEnemySpike += TakeDamage;
            EnemyCollisions.OnPlayerSquash += TakeDamage;
        }
    
        private void OnDisable()
        {
            EnemyCollisions.OnEnemySpike -= TakeDamage;
            EnemyCollisions.OnPlayerSquash -= TakeDamage;
        }
    

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            CanTakeDamage = true;
        }

        private void TakeDamage()
        {
        
            if (!CanTakeDamage)
            {
                return;
            }
        
            OnDamage?.Invoke(); 
        
            switch (enemyStatsHandler.runtimeStats.enemyHealth)
            {
                case > 0:
                    StartCoroutine(Invulnerability());
                    break;
                case <= 0:
                    Destroy(gameObject);
                    break;
            }
        }
        private IEnumerator Invulnerability()
        {
            CanTakeDamage = false;
        
            // Enemy sprite flashes in colour to indicate invulnerability
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0.25f, 0, 0.5f);
                yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
            }
        
            CanTakeDamage = true;
        }
    }
}
