using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpBar : HpBar
{
    public override int value => player.Mp;
    public override int maxValue => player.MMp;
}
