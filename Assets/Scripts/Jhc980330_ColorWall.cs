using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_ColorWall : MonoBehaviour
{
    [SerializeField] SpriteRenderer wall;
    [SerializeField] SpriteRenderer switch_on;
    [SerializeField] SpriteRenderer switch_off;
    [SerializeField] Color wallColor;

    private void ColorWall()
    {
        wall.color = wallColor;
        switch_on.color = wallColor;
        switch_off.color = wallColor;
    }

    private void Start()
    {
        ColorWall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wall.gameObject.SetActive(false);
            switch_on.gameObject.SetActive(true);
            switch_off.gameObject.SetActive(false);
        }
    }
}
