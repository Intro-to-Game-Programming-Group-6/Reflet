using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePortal : MonoBehaviour
{
    SpriteRenderer sprite;

    public bool allEnemiesDies;
    [SerializeField] private Color closeColor;
    [SerializeField] private Color openColor;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (allEnemiesDies)
        {
            sprite.color = openColor;
        }
        else
        {
            sprite.color = closeColor;
        }
    }

}
