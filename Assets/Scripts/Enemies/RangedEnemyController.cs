using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RangedEnemyController : EnemyController
{
    private const float edgeThreshold = 2.0f;

    public GameObject bullet;
    private GameObject bullets;
    public float approachRange = 30.0f;
    public float shootingRange = 15.0f;
    public float fleeRange = 8.0f;
    
    public float fleeSpeed = 3.0f;
    public float approachSpeed = 1.5f;
    
    public float shootingCooldown = 1.5f;
    private float nextShot = -1.0f;
    private float xOffset = 1.0f;

    protected override void Start()
    {
        base.Start();
        bullets = GameObject.Find("Bullets");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!IsGrounded())
        {
            return;
        }

        base.Update();

        bool isFleeing = false;
        if (dist <= fleeRange)
        {
            isFleeing = true;
            if(currentPlatform != null)
            {
                if (dir == -1)
                {
                    // right
                    float rightEdge = currentPlatform.bounds.center.x + currentPlatform.bounds.extents.x;
                    //Debug.Log(rightEdge - transform.position.x);
                    if (rightEdge - transform.position.x <= edgeThreshold)
                    {
                        isFleeing = false;
                    }
                }
                else
                {
                    // left
                    float leftEdge = currentPlatform.bounds.center.x - currentPlatform.bounds.extents.x;
                    //Debug.Log(transform.position.x - leftEdge);
                    if (transform.position.x - leftEdge <= edgeThreshold)
                    {
                        isFleeing = false;
                    }
                }
            }
        }

        if (isFleeing)
        {
            rb.velocity = new Vector2(-dir * fleeSpeed, rb.velocity.y);
        }
        else if (dist <= shootingRange)
        {
            shoot(new Vector2(player.transform.position.x, player.transform.position.y));
        }
        else if (base.dist <= approachRange)
        {
            rb.velocity = new Vector2(dir * fleeSpeed, rb.velocity.y);
        }
    }

    void shoot(Vector2 target)
    {
        if(base.time > nextShot)
        {
            Instantiate(bullet, new Vector3(transform.position.x + base.dir * xOffset, transform.position.y, 0.0f), Quaternion.identity,bullets.transform);
            nextShot = base.time + shootingCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, approachRange);

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, shootingRange);

        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.forward, fleeRange);
    }
}
