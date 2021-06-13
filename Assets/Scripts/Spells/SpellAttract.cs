using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAttract : Spell
{
    private const float spellCooldown = 30.0f;
    private const float spellManaCost = 200.0f;
    private const float range = 5.0f;
    private const float force = 100.0f;
    private const float delay = 0.3f;

    public SpellAttract() : base(spellCooldown, spellManaCost)
    {
    }

    protected override void Execute(BallController ball)
    {
        ball.Attract(range, force, delay);
    }
}