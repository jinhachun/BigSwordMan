using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    public float shakeForce = 1f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void CameraShake(CinemachineImpulseSource source)
    {
        source.GenerateImpulseWithForce(shakeForce);
    }
}
