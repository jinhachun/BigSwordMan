using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCoroutineScript : MonoBehaviour
{
    Transform _playerTransform;
    Player _player;

    float dashSpeed = 1f;
    float dashRange = 2f;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _playerTransform = _player.transform;
    }
    public void MeleeAttack()
    {

    }
    public void Dash()
    {

    }
    public void RangeAttack()
    {

    }
    
}
