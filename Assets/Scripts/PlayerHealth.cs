using System;
using Enemies.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public float currentHealth;
    private static Slider _healthSlider;
    private static TMP_Text _diedTitle;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    

    private Animator _anim;
    private AudioSource _playerAudio;
    private PlayerMovement _playerMovement;
    //private bool _isDead;
    private bool _damaged;
    private static readonly int Die = Animator.StringToHash("Die");

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        _playerMovement = GetComponent<PlayerMovement>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        if (_damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
            // damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        _damaged = false;
    }

    public static void SetHealthSlider(Slider slider)
    {
        _healthSlider = slider;
    }
    public static void SetDiedTitle(TMP_Text diedTitle)
    {
        _diedTitle = diedTitle;
    }

    public void TakeDamage(float amount)
    {
  
        _damaged = true;
        currentHealth -= amount;
        Debug.Log($"V1: {currentHealth}");
        _healthSlider.value = currentHealth;
        // _playerAudio.Play();
        Debug.Log($"TAKEDMG: {currentHealth}");
        if (!(currentHealth < 1)) return;
        Debug.Log($"WHY YOU NOT DYING: {currentHealth}");
        Death();
    }

    void Death()
    {
        Debug.Log($"DEATH: {currentHealth}");
        //_isDead = true;
        // _anim.SetTrigger(Die);
        // _playerAudio.clip = deathClip;
        // _playerAudio.Play();
        _playerMovement.enabled = false;
        _diedTitle.enabled = true;
        // EnemyAiMelee.StopGame();
        PauseMenu.SetState(State.Fail);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    
}