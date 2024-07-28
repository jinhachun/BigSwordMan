using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_Spring : MonoBehaviour
{
    public float springForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, springForce),ForceMode2D.Impulse);
        }
    }
}
