using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEnflame : Spell
{
    private const float spellCooldown = 30.0f;
    private const float spellManaCost = 150.0f;
    private const float damageToPlayer = 5.0f;
    private const float damageToEnemy = 2.0f;
    private const float burningDuration = 15.0f;

    public SpellEnflame() : base(spellCooldown, spellManaCost)
    {
        
    }

    protected override void Execute(BallController ball)
    {
        ball.SetBurningFor(burningDuration, damageToPlayer, damageToEnemy);
    }
}
