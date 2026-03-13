using UI;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource completionSource;
    public AudioSource jumpSource;
    public AudioSource openSource;
    public AudioSource closeSource;
    public AudioSource damageSource;
    public AudioSource upSource;
    public AudioSource downSource;
    public AudioSource indexSource;
    public AudioSource errorSource;

    private void OnEnable()

    {
        GameManager.OnMenuOpen += PlayOpen;
        GameManager.OnMenuClose += PlayClose;
        GameManager.OnError += PlayError;
        StatBlockInput.OnError += PlayError;
        StatBlockInput.OnUp += PlayUp;
        StatBlockInput.OnDown += PlayDown;
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
        StatBlockInput.OnError -= PlayError;
        StatBlockInput.OnUp -= PlayUp;
        StatBlockInput.OnDown -= PlayDown;
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
        GameObject openAudio = GameObject.Find("Open");
        if (openAudio)
        {
            openSource = openAudio.GetComponent<AudioSource>();
        }
        
        GameObject closeAudio = GameObject.Find("Close");
        if (closeAudio)
        {
            closeSource = closeAudio.GetComponent<AudioSource>();
        }
        
        GameObject upAudio = GameObject.Find("Up");
        if (upAudio)
        {
            upSource = upAudio.GetComponent<AudioSource>();
        }
        
        GameObject downAudio = GameObject.Find("Down");
        if (downAudio)
        {
            downSource = downAudio.GetComponent<AudioSource>();
        }
        
        GameObject indexAudio = GameObject.Find("Index");
        if (indexAudio)
        {
            indexSource = indexAudio.GetComponent<AudioSource>();
        }
        
        GameObject errorAudio = GameObject.Find("Error");
        if (errorAudio)
        {
            errorSource = errorAudio.GetComponent<AudioSource>();
        }

        
        GameObject jumpAudio = GameObject.Find("Jump");
        if (jumpAudio)
        {
            jumpSource = jumpAudio.GetComponent<AudioSource>();
        }
        
        GameObject damageAudio = GameObject.Find("Damage");
        if (damageAudio)
        {
            damageSource = damageAudio.GetComponent<AudioSource>();
        }
        
        GameObject completionAudio = GameObject.FindGameObjectWithTag("Completion Audio");
        if (completionAudio)
        {
            completionSource = completionAudio.GetComponent<AudioSource>();
        }
    }

    public void PlayOpen()
    {
        openSource.Play();
    }

    public void PlayClose()
    {
        closeSource.Play();
    }

    public void PlayDamage()
    {
        damageSource.Play();
    }

    public void PlayUp()
    {
        upSource.Play();
    }

    public void PlayDown()
    {
        downSource.Play();
    }

    public void PlayIndex()
    {
        indexSource.Play();
    }

    public void PlayError()
    {
        errorSource.Play();
    }

    public void PlayJump()
    {
        jumpSource.Play();
    }

    public void PlayCompletion()
    {
        completionSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}