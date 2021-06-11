using System;
using System.Security.Cryptography;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector2 pos;
    private Vector2 vel;
    private Vector2 acc;
    private GameObject player;
    private Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        target = player.transform.position;
        Vector2 force = speed * (target - pos);
        iterate(force);
    }

    void iterate(Vector2 f)
    {
        acc = f;
        vel += acc;
        transform.Translate(vel);
    }
}
