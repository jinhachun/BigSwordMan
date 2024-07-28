using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    protected Player player;
    Slider slider;
    virtual public int value => player.Hp;
    virtual public int maxValue => player.MHp;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        StartCoroutine(nameof(HpBarUpdate));
    }
    IEnumerator HpBarUpdate()
    {
        while (true)
        {
            slider.value = value;
            slider.maxValue = maxValue;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
