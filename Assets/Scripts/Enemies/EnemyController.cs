using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Groundable
{
    protected GameObject player;
    protected Rigidbody2D rb;
    protected float time = 0.0f;
    protected float dist = 0.0f;
    protected int dir = -1;
    public float health = 10.0f;

    [HideInInspector]
    public Collider2D currentPlatform;
    
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        //Debug.Log("Distance: " + dist);
        time = time + Time.deltaTime;
        dist = Vector2.Distance(player.transform.position,transform.position);
        dir = (player.transform.position.x < transform.position.x ? -1 : 1);

        currentPlatform = GetPlatform();
    }

    public virtual bool receiveDamage(float amount)
    {
        health -= amount;
        if(health < 0.0f)
        {
            destruct();
            return true;
        }
        return false;
    }

    public virtual void destruct()
    {
        Destroy(gameObject);
    }
}
