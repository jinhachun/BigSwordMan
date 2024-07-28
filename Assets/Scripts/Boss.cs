using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject sprite;
    Transform player;
    Player _player;
    public int hp;
    public int mHp;
    public float hpPer => hp / mHp * 100;
    public bool isWatchingLeft = true;
    [SerializeField] private bool isHurt;


    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _player = player.GetComponent<Player>();
    }
    public void Update()
    {
    }
    public void Hurt()
    {
        this.hp -= _player.Att;
    }
    public void TurnCheck()
    {
        Debug.Log("≈œ√º≈© =====================================================");
        Debug.Log($"{this.transform.rotation.y} ==== {player.transform.position} === {transform.position.x} ");
        if (this.transform.rotation.y < 0 && player.transform.position.x<this.transform.position.x)
        {
            isWatchingLeft = true;
            Turn();
        }
        else if (this.transform.rotation.y >= 0 && player.transform.position.x > this.transform.position.x)
        {
            isWatchingLeft = false;
            Turn();
        }
    }
    private void Turn()
    {

        if (player.position.x > this.transform.position.x)
        {
            Vector3 rotator = new Vector3(this.transform.rotation.x, 180f, this.transform.rotation.z);
            this.transform.rotation = Quaternion.Euler(rotator);
        }
        else
        {
            Vector3 rotator = new Vector3(this.transform.rotation.x, 0f, this.transform.rotation.z);
            this.transform.rotation = Quaternion.Euler(rotator);
        }
    }
}
