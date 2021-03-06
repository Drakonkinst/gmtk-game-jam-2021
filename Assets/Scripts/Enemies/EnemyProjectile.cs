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
    public float damage = 5.0f;
    private float startTime;
    private Vector3 velocityDir;

    public AudioClip hitSound;

    //Decided against using Rigidbody2D because of unexpected behavior
    void Start()
    {
        Vector3 playerPos = PlayerStatus.instance.transform.position;
        velocityDir = (playerPos - transform.position).normalized;
        transform.LookAt(playerPos);
        transform.Rotate(new Vector3(0.0f, 90.0f, 90.0f));
        startTime = Time.time;
    }

    void Update()
    {
        if (InputManager.instance.ball.IsOnBall(transform.position) || Time.time > startTime + liveTime)
        {
            SoundManager.Instance.Play(hitSound, transform.position, 0.5f, 0.8f);
            DestructSelf();
            return;
        }
        transform.position += velocityDir * speed;
    }

    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision with " + col.collider.tag);
        if(col.collider.tag != "Enemy")
        {
            DestructSelf();
        }
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Enemy")
        {
            if(col.tag == "Player")
            {
                PlayerStatus.instance.Damage(damage);
            }
            DestructSelf();
        }
    }

    void DestructSelf()
    {
        Destroy(gameObject);
    }
}
