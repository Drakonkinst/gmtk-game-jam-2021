using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShoot : Spell
{
    private const float spellCooldown = 5.0f;
    private const float spellManaCost = 100.0f;
    private const float force = 500.0f;
    // min velocity to allow player to shoot, i.e. can't shoot near 0 velocity)
    private const float minVelocity = 0.5f;

    public SpellShoot() : base(spellCooldown, spellManaCost)
    {

    }

    protected override bool Execute(BallController ball)
    {
        Vector2 dir = ball.GetComponent<Rigidbody2D>().velocity;
        if(ball.currentState == BallController.State.Controlled)
        {
            SetResultText("Cannot use this while controlling orb!");
            return false;
        }
        if(dir.sqrMagnitude < minVelocity * minVelocity)
        {
            SetResultText("Orb must be moving!");
            return false;
        }

        ball.ShootBall(dir.normalized, force);
        return true;
    }
}
