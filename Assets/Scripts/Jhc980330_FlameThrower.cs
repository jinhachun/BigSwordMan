using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_FlameThrower : MonoBehaviour
{
    public float startRate;
    public float fireRate;
    [SerializeField] GameObject Flame;
    void Start()
    {
        Invoke(nameof(StopFire), startRate);
    }

    void Fire()
    {
        Flame.SetActive(true);
        Invoke(nameof(StopFire), fireRate);
    }
    void StopFire()
    {
        Flame.SetActive(false);
        Invoke(nameof(Fire),fireRate+1f);
    }
}
