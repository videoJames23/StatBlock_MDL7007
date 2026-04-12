using System.Collections;
using Enemy;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Player
{
    public class PlayerDamage : MonoBehaviour
    {
        // Responsible for:
        // determining whether player should take damage
        // &
        // informing StatBlockChangesP of damage taken
        // &
        // setting and disabling player invulnerability
        // &
        // flashing player sprite during invulnerability
        
        [SerializeField] private PlayerStatValues  playerStats;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PlayerStatsHandler playerStatsHandler;
        
        private bool canTakeDamage;
        private const float iFramesDuration = 1; // invulnerability duration in seconds
        private const int numberOfFlashes = 5; // number of sprite flashes to indicate invulnerability
    
        public delegate void Damage();
        public static event  Damage OnDamage;

        private void OnEnable()
        {
            EnemyCollisions.OnEnemyAttack += TakeDamage;
        }

        private void OnDisable()
        {
            EnemyCollisions.OnEnemyAttack -= TakeDamage;
        }
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            canTakeDamage = true;
            Physics2D.IgnoreLayerCollision(10, 11, false);
            // ensure player can collide with enemy and spikes
        }
    
        public void TakeDamage()
        {
            if (!canTakeDamage)
            {
                return;
            }
        
            OnDamage?.Invoke(); 
        
            switch (playerStatsHandler.runtimeStats.playerHealth)
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
            canTakeDamage = false;
            Physics2D.IgnoreLayerCollision(10, 11, true);
            
            for (int i = 0; i < numberOfFlashes; i++)
            {
                // Enemy sprite flashes in colour to indicate invulnerability
                spriteRenderer.color = new Color(0, 0.25f, 1, 0.5f);
                yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
                spriteRenderer.color = Color.blue;
                yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
            }
        
            Physics2D.IgnoreLayerCollision(10, 11, false);
            canTakeDamage = true;
        }
    }
}
