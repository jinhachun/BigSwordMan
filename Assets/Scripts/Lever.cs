using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] Transform LeverButton;
    public bool isSwitchOn;
    // ����ġ�� on �Ǿ��� ���� �̺�Ʈ
    public UnityEvent customEventON;

    private void Update()
    {
        if (isSwitchOn && LeverButton.rotation.z > -45f)
        {
            LeverButton.rotation = Quaternion.Euler(0f, 0f, -45f);
        }
    }
    public void SetSwitchOn()
    {
        isSwitchOn = true;
        customEventON.Invoke();
    }

}
