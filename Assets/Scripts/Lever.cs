using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] Transform LeverButton;
    public bool isSwitchOn;
    // 스위치가 on 되었을 때의 이벤트
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
