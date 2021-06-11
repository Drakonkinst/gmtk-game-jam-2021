using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    public float dampTime = 0.5f;
    private Vector3 pos;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        dampTime = (2 / Vector2.Distance(player.position,transform.position));
        pos = new Vector3(player.position.x, player.position.y,-10.0f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, pos, ref velocity, dampTime);
    }
}
