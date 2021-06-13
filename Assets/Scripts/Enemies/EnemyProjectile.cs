using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float liveTime = 4.0f;
    public float speed = 0.1f;
    private Vector2 lastPosition;
    private float time = 0.0f;
    //Decided against using Rigidbody2D because of unexpected behavior
    void Start()
    {
        lastPosition = GameObject.FindWithTag("Player").transform.position;
        transform.LookAt(lastPosition);
        transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
    }

    void Update()
    {
        time = time + Time.deltaTime;
        if (time <= liveTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastPosition, speed);
        }
        else
        {
            DestructSelf();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag != "Enemy")
        {
            DestructSelf();
        }
    }

    /*void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Enemy")
        {
            DestructSelf();
        }
    }*/

    void DestructSelf()
    {
        Destroy(gameObject);
    }
}
