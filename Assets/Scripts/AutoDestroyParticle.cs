using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoDestroyParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // 파티클 시스템이 완료되었는지 확인
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}