using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpBar : HpBar
{
    [SerializeField] Boss _boss;
    public override int value => _boss.hp;
    public override int maxValue => _boss.mHp;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(value + " " + maxValue);
    }
}
