using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_FollowPlayer : MonoBehaviour
{
    public Jhc980330_PlayerController player;
    private Transform playerTransform;
    private Vector3 offset = new Vector3(0, 0f, -10f);
    private float offsetYWhileGround = 1f;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.gameObject.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if (player.isGrounded())
        {
            offset = new Vector3(0, offsetYWhileGround, -10f);
        }
        else
        {
            offset = new Vector3(0, -offsetYWhileGround/2, -10f);
        }
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref velocity,smoothTime);
    }
}
