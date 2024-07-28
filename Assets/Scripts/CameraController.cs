using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject normalCamera;
    [SerializeField] GameObject bossCamera;

    Player player;
    bool isBossCamera => false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        if (player.isBossFighting)
        {
            normalCamera.SetActive(false);
            bossCamera.SetActive(true);
        }
        else
        {
            normalCamera.SetActive(true);
            bossCamera.SetActive(false);
        }
    }
}
