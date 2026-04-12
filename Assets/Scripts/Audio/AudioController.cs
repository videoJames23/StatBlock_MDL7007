using Enemy;
using Player;
using StatBlock;
using UI;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Responsible for:
    // Playing audio files on subscribed events
    
    [SerializeField] private AudioSource completionSource;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource openSource;
    [SerializeField] private AudioSource closeSource;
    [SerializeField] private AudioSource damageSource;
    [SerializeField] private AudioSource upSource;
    [SerializeField] private AudioSource downSource;
    [SerializeField] private AudioSource indexSource;
    [SerializeField] private AudioSource errorSource;
    
    private void OnEnable()
    {
        // Start screen
        MainMenuScreen.OnButtonClicked += PlayUp;
        LevelSelectScreen.OnBack += PlayDown;
        CreditsScreen.OnBack += PlayDown;
        
        // StatBlock open/close
        StatBlockUI.OnMenuOpen += PlayOpen;
        StatBlockUI.OnMenuClose += PlayClose;
        
        // StatBlock error
        StatBlockChangesP.OnError += PlayError;
        StatBlockChangesE.OnError += PlayError;
        
        // Stat changes
        StatBlockChangesP.OnUp += PlayUp;
        StatBlockChangesP.OnDown += PlayDown;
        StatBlockChangesE.OnUp += PlayUp;
        StatBlockChangesE.OnDown += PlayDown;
        
        // Stat selection
        StatBlockInput.OnIndexChanged += PlayIndex;
        
        // Player sfx
        PlayerMovement.OnJumpEvent += PlayJump;
        PlayerDamage.OnDamage += PlayDamage;
        PlayerCollisions.OnCompletion += PlayCompletion;
        
        // Enemy sfx
        EnemyDamage.OnDamage += PlayDamage;
    }

    private void OnDisable()
    {
        // Start screen
        MainMenuScreen.OnButtonClicked -= PlayUp;
        LevelSelectScreen.OnBack -= PlayDown;
        CreditsScreen.OnBack -= PlayDown;
        
        // StatBlock open/close
        StatBlockUI.OnMenuOpen -= PlayOpen;
        StatBlockUI.OnMenuClose -= PlayClose;
        
        // StatBlock error
        StatBlockChangesP.OnError -= PlayError;
        StatBlockChangesE.OnError -= PlayError;
        
        // Stat changes
        StatBlockChangesP.OnUp -= PlayUp;
        StatBlockChangesP.OnDown -= PlayDown;
        StatBlockChangesE.OnUp -= PlayUp;
        StatBlockChangesE.OnDown -= PlayDown;
        
        // Stat selection
        StatBlockInput.OnIndexChanged -= PlayIndex;
        
        // Player sfx
        PlayerMovement.OnJumpEvent -= PlayJump;
        PlayerDamage.OnDamage -= PlayDamage;
        PlayerCollisions.OnCompletion -= PlayCompletion;
        
        // Enemy sfx
        EnemyDamage.OnDamage -= PlayDamage;
    }
    
    
    private static void Play(AudioSource source)
    {
        if (!source || !source.isActiveAndEnabled)
            return;

        source.Play();
    }
    private void PlayOpen() => Play(openSource);
    private void PlayClose() => Play(closeSource);
    private void PlayDamage() => Play(damageSource);
    private void PlayUp() => Play(upSource);
    private void PlayDown() => Play(downSource);
    private void PlayIndex() => Play(indexSource);
    private void PlayError() => Play(errorSource);
    private void PlayJump() => Play(jumpSource);
    private void PlayCompletion() => Play(completionSource);

}