using System.Collections;
using Scriptable_Objects.StatInfo;
using UnityEngine;

namespace Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        private EnemyStatsHandler enemyStatsHandler;

        [SerializeField] private EnemyStatValues enemyStats;
        
        private SpriteRenderer spriteRenderer;
        private const float iFramesDuration = 1;
        private const int numberOfFlashes = 5;
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
            enemyStatsHandler = GetComponent<EnemyStatsHandler>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            CanTakeDamage = true;
        }

        private void TakeDamage()
        {
        
            if (!CanTakeDamage)
            {
                return;
            }
        
            OnDamage?.Invoke(); 
            CanTakeDamage = false;
        
            if (enemyStatsHandler.runtimeStats.enemyHealth > 0)
            {
                StartCoroutine(Invulnerability());
            }
        
            else if (enemyStatsHandler.runtimeStats.enemyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        private IEnumerator Invulnerability()
        {
            CanTakeDamage = false;
        
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
