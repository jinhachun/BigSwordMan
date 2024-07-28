using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScaleAdjuster : MonoBehaviour
{
    public Transform ground; // Ground ������Ʈ
    public Transform wall; // Wall ������Ʈ
    public Transform ceiling; // Wall ������Ʈ

    public float xscale = 1.0f; // �Է¹��� xscale ��
    public float yscale = 1.0f; // �Է¹��� yscale ��

    void Start()
    {
        AdjustScale();
    }

    [ContextMenu("����������")]
    public void AdjustScale()
    {
        float originalGroundYScale = ground.localScale.y;
        float originalCeilingYScale = ceiling.localScale.y;

        Vector3 wallScale = wall.localScale;
        wallScale.x = xscale;
        wallScale.y = yscale;
        wall.localScale = wallScale;

        Vector3 parentScale = transform.localScale;
        parentScale.x = xscale / wall.localScale.x;
        parentScale.y = yscale / wall.localScale.y;
        transform.localScale = parentScale;

        Vector3 groundScale = ground.localScale;
        groundScale.x = wall.localScale.x; // Ground�� xscale�� Wall�� ����
        groundScale.y = originalGroundYScale; // Ground�� yscale ����
        ground.localScale = groundScale;

        ground.localPosition = wall.localPosition + new Vector3(0,wall.localScale.y/2+0.5f,0);
   
        Vector3 ceilingScale = ceiling.localScale;
        ceilingScale.x = wall.localScale.x; // Ground�� xscale�� Wall�� ����
        ceilingScale.y = originalCeilingYScale; // Ground�� yscale ����
        ceiling.localScale = ceilingScale;

        ground.localPosition = wall.localPosition + new Vector3(0, wall.localScale.y / 2 , 0);
        ceiling.localPosition = wall.localPosition - new Vector3(0, wall.localScale.y / 2, 0);
    }
}
