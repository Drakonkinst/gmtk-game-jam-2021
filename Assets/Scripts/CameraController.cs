using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private Transform target;
    public float dampTime = 0.5f;
    private Vector3 pos;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetTargetToPlayer();
    }

    private void Update()
    {
        dampTime = (2 / Vector2.Distance(target.position,transform.position));
        pos = new Vector3(target.position.x, target.position.y,-10.0f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, pos, ref velocity, dampTime);
    }

    public void SetTargetToPlayer()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    public void SetTargetToBall()
    {
        target = FindObjectOfType<BallController>().transform;
    }
}
