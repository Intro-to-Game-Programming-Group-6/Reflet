using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAreaCheck : MonoBehaviour
{
    SpriteRenderer sprite;

    public bool detectLife;
    [SerializeField] private Color show;
    [SerializeField] private Color hide;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (detectLife)
        {
            sprite.color = show;
        }
        else
        {
            sprite.color = hide;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            detectLife = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            detectLife = false;
        }
    }

}
