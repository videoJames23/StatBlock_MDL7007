using UI;
using UnityEngine;

public class AudioController : MonoBehaviour
{
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
        GameManager.OnMenuOpen += PlayOpen;
        GameManager.OnMenuClose += PlayClose;
        GameManager.OnError += PlayError;
        StatBlockChangesP.OnError += PlayError;
        StatBlockChangesE.OnError += PlayError;
        StatBlockChangesP.OnUp += PlayUp;
        StatBlockChangesP.OnDown += PlayDown;
        StatBlockChangesE.OnUp += PlayUp;
        StatBlockChangesE.OnDown += PlayDown;
        StatBlockInput.OnIndex += PlayIndex;
        
        MainMenuScreen.OnButtonClicked += PlayUp;
        LevelSelectScreen.OnBack += PlayDown;
        CreditsScreen.OnBack += PlayDown;
        
        
        PlayerMovement.OnJumpEvent += PlayJump;
        PlayerDamage.OnDamage += PlayDamage;
        PlayerCollisions.OnCompletion += PlayCompletion;
        
        EnemyDamage.OnDamage += PlayDamage;
    }

    private void OnDisable()
    {
        GameManager.OnMenuOpen -= PlayOpen;
        GameManager.OnMenuClose -= PlayClose;
        GameManager.OnError -= PlayError;
        StatBlockChangesP.OnError -= PlayError;
        StatBlockChangesE.OnError -= PlayError;
        StatBlockChangesP.OnUp -= PlayUp;
        StatBlockChangesP.OnDown -= PlayDown;
        StatBlockChangesE.OnUp -= PlayUp;
        StatBlockChangesE.OnDown -= PlayDown;
        StatBlockInput.OnIndex -= PlayIndex;
        
        MainMenuScreen.OnButtonClicked -= PlayUp;
        LevelSelectScreen.OnBack -= PlayDown;
        CreditsScreen.OnBack -= PlayDown;
        
        PlayerMovement.OnJumpEvent -= PlayJump;
        PlayerDamage.OnDamage -= PlayDamage;
        PlayerCollisions.OnCompletion -= PlayCompletion;
        
        EnemyDamage.OnDamage -= PlayDamage;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void PlayOpen()
    {
        openSource.Play();
    }

    private void PlayClose()
    {
        closeSource.Play();
    }

    private void PlayDamage()
    {
        damageSource.Play();
    }

    private void PlayUp()
    {
        upSource.Play();
    }

    private void PlayDown()
    {
        downSource.Play();
    }

    private void PlayIndex()
    {
        indexSource.Play();
    }

    private void PlayError()
    {
        errorSource.Play();
    }

    private void PlayJump()
    {
        jumpSource.Play();
    }

    private void PlayCompletion()
    {
        completionSource.Play();
    }
}