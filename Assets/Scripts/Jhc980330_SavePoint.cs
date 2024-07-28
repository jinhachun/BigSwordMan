using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_SavePoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer flag;
    public bool isChkPoint;
    private void Start()
    {

    }
    public void Chk()
    {
        if (isChkPoint) return;
        isChkPoint = true;
        flag.gameObject.SetActive(true);
        Jhc980330_GameManager.Instance.SetSavePoint(this.gameObject);
        Jhc980330_GameManager.Instance.savePoint = new Vector2(transform.position.x,transform.position.y+2);
    }
    public void Off()
    {
        isChkPoint = false;
        flag.gameObject.SetActive(false);
    }
    public void Update()
    {
        if(!isChkPoint) return;
        if(Jhc980330_GameManager.Instance.isSavePoint(this.gameObject))
        {
            Debug.Log("¤¾¤·");
            Off();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Chk();
    }
}
