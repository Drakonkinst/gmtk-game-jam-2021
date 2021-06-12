using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    public float chaseRange = 20.0f;
    public float lungeRange = 5.0f;
    public float chaseSpeed = 10.0f;
    public float lungeForce = 5.0f;
    public float lungeCooldown = 1.0f;
    private float nextLunge = -1.0f;
    //private int behaviorState = 0; // 0 inactive, 1 chasing, 2 lunging
    protected override void Update()
    {
        if(!IsGrounded())
        {
            return;
        }

        base.Update();

        if (base.dist <= lungeRange) // Player entered lunging range
        {
            if(base.time > nextLunge)
            {
                base.rb.AddForce(new Vector2(base.dir * lungeForce, 0.0f), ForceMode2D.Impulse);
                nextLunge = base.time + lungeCooldown;
            }

        }
        else if (base.dist <= chaseRange) // Player entered chasing range
        {
            rb.velocity = new Vector2(base.dir * chaseSpeed, base.rb.velocity.y);
        }
    }
}
