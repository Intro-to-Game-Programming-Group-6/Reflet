using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
//Require: Player, EnemyManager, Player

public class EffectManager : MonoBehaviour
{
    AudioSource playerAudioSource;

    public static EffectManager instance;

    public UnityEvent bullet_reflected;
    public UnityEvent shield_broken;
    public UnityEvent player_hurt;
    public UnityEvent player_die;
    public UnityEvent stamina_depleted;
    public UnityEvent bullet_captured;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static EffectManager GetInstance()
    {
        return instance;
    }

    #region Effect
    [Header("Visual Effects")]
    [SerializeField] EffectCollection effectSettings;
    private Dictionary<string, Tuple<GameObject, GameObject, GameObject>> effectDict;    

    [Header("Non-Enemy effects")]
    [SerializeField] private GameObject healVFX;
    [SerializeField] private AudioClip bulletBounceSFX;
    [SerializeField] private AudioClip shieldBreakSFX;
    [SerializeField] private AudioClip playerWalkSFX;
    [SerializeField] private AudioClip playerDashSFX;
    [SerializeField] private AudioClip playerHurtSFX;
    [SerializeField] private AudioClip playerDeathSFX;
    [SerializeField] private AudioClip staminaDepletedSFX;
    [SerializeField] private AudioClip bulletCapturedSFX;
    #endregion


    private void Start() {
        effectDict = effectSettings.Dict();
        playerAudioSource = PlayerControlScript.GetInstance().NormalPitchSource;
        EnemyManager.GetInstance().EnemyHurt.AddListener(SpawnHurtEffect);
        EnemyManager.GetInstance().EnemyDie.AddListener(SpawnDeathEffect);
        EnemyManager.GetInstance().EnemyShoot.AddListener(SpawnShootingEffect);
        bullet_reflected.AddListener(BulletReflectSound);
        shield_broken.AddListener(BrokenShield);
        player_hurt.AddListener(PlayPlayerHurt);
        player_die.AddListener(PlayerDeadSound);
        stamina_depleted.AddListener(OutOfStamina);
        bullet_captured.AddListener(BulletCaptured);
    }

    private void OnDisable() {
        EnemyManager.GetInstance().EnemyHurt.RemoveListener(SpawnHurtEffect);
        EnemyManager.GetInstance().EnemyDie.RemoveListener(SpawnDeathEffect);
        EnemyManager.GetInstance().EnemyShoot.RemoveListener(SpawnShootingEffect);
        bullet_reflected.RemoveListener(BulletReflectSound);
        shield_broken.RemoveListener(BrokenShield);
        player_hurt.RemoveListener(PlayPlayerHurt);
        player_die.RemoveListener(PlayerDeadSound);
        stamina_depleted.RemoveListener(OutOfStamina);
        bullet_captured.RemoveListener(BulletCaptured);
    }

    //Positional Effects
    public void SpawnShootingEffect(Vector3 position, string enemyName) {
        Instantiate(effectDict[enemyName].Item1, position, Quaternion.identity);
    }

    public void SpawnHurtEffect(Vector3 position, string enemyName) {
        Instantiate(effectDict[enemyName].Item2, position, Quaternion.identity);
    }

    public void SpawnDeathEffect(Vector3 position, string enemyName) {
        Instantiate(effectDict[enemyName].Item3, position, Quaternion.identity);
    }

    public void SpawnHealEffect(Vector3 position, string enemyName) {
        Instantiate(healVFX, position, Quaternion.identity);
    }

    public void BulletReflectSound()
    {
        playerAudioSource.PlayOneShot(bulletBounceSFX);
    }

    public void BrokenShield()
    {
        playerAudioSource.PlayOneShot(shieldBreakSFX);
    }

    public void PlayPlayerHurt()
    {
        playerAudioSource.PlayOneShot(playerHurtSFX);
    }

    public void PlayerDeadSound()
    {
        playerAudioSource.PlayOneShot(playerDeathSFX);
    }

    public void OutOfStamina()
    {
        playerAudioSource.PlayOneShot(staminaDepletedSFX);
    }

    public void BulletCaptured()
    {
        playerAudioSource.PlayOneShot(bulletCapturedSFX);
    }
    //Non-positional effects

    // @Booby plz move these to the player script
    /*
    public void PlayPlayerMove(InputAction.CallbackContext context) {
        if(context.performed) {
            playerAudioSource.loop = true;
            playerAudioSource.clip = playerWalkSFX;
            playerAudioSource.Play();
        } else if (context.canceled) {
            playerAudioSource.Stop();
        }
    }

    public void PlayBulletBounce() {
        playerAudioSource.PlayOneShot(bulletBounceSFX);
    }
    */
}
