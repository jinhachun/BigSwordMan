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
        // ��ƼŬ �ý����� �Ϸ�Ǿ����� Ȯ��
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}