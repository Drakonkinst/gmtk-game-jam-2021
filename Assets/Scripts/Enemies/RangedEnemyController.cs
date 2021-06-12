using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    public float approachRange = 30.0f;
    public float shootingRange = 20.0f;
    public float fleeRange = 10.0f;
    
    public float fleeSpeed = 3.0f;
    public float approachSpeed = 1.5f;

    public float shootingCooldown = 1.5f;
    private float nextShot = -1.0f;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(base.dist <= fleeRange)
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
            Debug.Log("SHOT");
            nextShot = base.time + shootingCooldown;
        }
    }
}
