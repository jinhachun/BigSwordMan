using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScaleAdjuster : MonoBehaviour
{
    public Transform ground; // Ground 오브젝트
    public Transform wall; // Wall 오브젝트
    public Transform ceiling; // Wall 오브젝트

    public float xscale = 1.0f; // 입력받은 xscale 값
    public float yscale = 1.0f; // 입력받은 yscale 값

    void Start()
    {
        AdjustScale();
    }

    [ContextMenu("스케일적용")]
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
        groundScale.x = wall.localScale.x; // Ground의 xscale을 Wall과 같게
        groundScale.y = originalGroundYScale; // Ground의 yscale 유지
        ground.localScale = groundScale;

        ground.localPosition = wall.localPosition + new Vector3(0,wall.localScale.y/2+0.5f,0);
   
        Vector3 ceilingScale = ceiling.localScale;
        ceilingScale.x = wall.localScale.x; // Ground의 xscale을 Wall과 같게
        ceilingScale.y = originalCeilingYScale; // Ground의 yscale 유지
        ceiling.localScale = ceilingScale;

        ground.localPosition = wall.localPosition + new Vector3(0, wall.localScale.y / 2 , 0);
        ceiling.localPosition = wall.localPosition - new Vector3(0, wall.localScale.y / 2, 0);
    }
}
