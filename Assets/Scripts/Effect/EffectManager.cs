using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Require: Player, EnemyManager, Player

public class EffectManager : MonoBehaviour
{
    AudioSource playerAudioSource;

    public static EffectManager instance;
    //public static EffectManager GetInstance() { return instance; } 

    private void OnEnable() {
        if(instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
            return;
        }
        playerAudioSource = GameObject.Find("Player")?.GetComponent<AudioSource>();
    }

    #region Effect
    [Header("Visual Effects")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject hurtVFX;
    [SerializeField] private GameObject healVFX;
    [SerializeField] private GameObject shootVFX;

    [Header("Sound Only")]
    [SerializeField] private AudioClip bulletBounceSFX;
    [SerializeField] private AudioClip playerWalkSFX;
    #endregion

    private void Start() {
        EnemyManager.GetInstance().EnemyHurt.AddListener(SpawnHurtEffect);
        EnemyManager.GetInstance().EnemyDie.AddListener(SpawnDeathEffect);
        EnemyManager.GetInstance().EnemyShoot.AddListener(SpawnShootingEffect);
    }

    private void Initialize() {
        //deathVFX.Add("Sentry", (GameObject)Resources.Load("", typeof(GameObject)));
    }

    //Positional Effects

    public void SpawnDeathEffect(Vector3 position, string enemyName) {
        Instantiate(deathVFX, position, Quaternion.identity);
    }

    public void SpawnHurtEffect(Vector3 position, string enemyName) {
        Instantiate(hurtVFX, position, Quaternion.identity);
    }

    public void SpawnHealEffect(Vector3 position, string enemyName) {
        Instantiate(healVFX, position, Quaternion.identity);
    }

    public void SpawnShootingEffect(Vector3 position, string enemyName) {
        Instantiate(shootVFX, position, Quaternion.identity);
    }

    //Non-positional effects

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

}
