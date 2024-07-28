using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private Jhc980330_PlayerController _player;
    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.GetComponent<Jhc980330_PlayerController>();
        _isFacingRight = (_player.playerDirection.x<0);
    }
    private void Update()
    {
        if (_player.isGroundedTight())
        {
            this.transform.position = new Vector2(_player.transform.position.x,_player.transform.position.y+1f);
        }else
            transform.position = _player.transform.position;
    }
    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime/_flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f,yRotation, 0f);
            yield return null;
        }
    }
    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
        {
            return 180f;
        }else
        {
            return 0f;
        }
    }
}
