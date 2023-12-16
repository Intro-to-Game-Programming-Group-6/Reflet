using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private static Exit instance;

    private Collider2D col;
    private SpriteRenderer sprite;
    private string nextScene = "MainMenu";

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Exit GetInstance()
    {
        return instance;
    }

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;

        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    public void EnableExit()
    {
        col.enabled = true;
        sprite.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        Transition.GetInstance().StartTransition(true, Color.black, 1.5f, 1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextScene);
    }
}
