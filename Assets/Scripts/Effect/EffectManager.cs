using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Manager", menuName = "Managers/Effects")]
public class EffectManager : ScriptableObject
{
    GameObject player;
    private void OnEnable() {
        player = GameObject.Find("Player");
    }

    #region Movement Variables
    [Header("Visual Effects")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject hurtVFX;

    [Header("Sound Only")]
    [SerializeField] private AudioClip shootingSFX;
    #endregion


    //Positional Effects

    public void SpawnDeathEffect(Vector3 position) {
        Instantiate(deathVFX, position, Quaternion.identity);
    }

    public void SpawnHurtEffect(Vector3 position) {
        Instantiate(hurtVFX, position, Quaternion.identity);
    }

    //Non-positional effects

    public void SpawnShootingEffect() {
        player.GetComponent<AudioSource>().PlayOneShot(shootingSFX);
    }
}
