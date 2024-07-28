using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_MovingObstacle : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;
    public GameObject ways;
    public Transform[] wayPoint;
    int pointIndex;
    int pointCount;
    int direction = 1;

    private void Awake()
    {
        wayPoint = new Transform[ways.transform.childCount];
        for(int i = 0; i < ways.transform.childCount; i++)
        {
            wayPoint[i] = ways.transform.GetChild(i).transform;
        }
    }
    private void Start()
    {
        pointCount = wayPoint.Length;
        pointIndex = 1;
        targetPos = wayPoint[pointIndex].transform.position;
    }
    private void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        if(transform.position == targetPos)
        {
            NextPoint();
        }
    }
    void NextPoint()
    {
        if(pointIndex == pointCount-1)
        {
            direction = -1;
        }
        if(pointIndex ==0)
        {
            direction = 1;
        }
        pointIndex += direction;
        targetPos = wayPoint[pointIndex].transform.position;
    }
}
