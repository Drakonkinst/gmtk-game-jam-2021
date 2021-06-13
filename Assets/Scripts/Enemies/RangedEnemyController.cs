using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    public GameObject bullet;
    public GameObject bullets;
    public float approachRange = 30.0f;
    public float shootingRange = 20.0f;
    public float fleeRange = 10.0f;
    
    public float fleeSpeed = 3.0f;
    public float approachSpeed = 1.5f;
    
    public float shootingCooldown = 1.5f;
    private float nextShot = -1.0f;
    private float xOffset = 2.0f;
    // Update is called once per frame
    protected override void Update()
    {
        if (!IsGrounded())
        {
            return;
        }

        base.Update();

        if (base.dist <= fleeRange)
        {
            base.rb.velocity = new Vector2(-base.dir * fleeSpeed, base.rb.velocity.y);
        }
        else if (base.dist <= shootingRange)
        {
            shoot(new Vector2(base.player.transform.position.x, base.player.transform.position.y));
        }
        else if (base.dist <= approachRange)
        {
            base.rb.velocity = new Vector2(base.dir * fleeSpeed, base.rb.velocity.y);
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
}
