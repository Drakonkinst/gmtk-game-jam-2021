using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRepel : Spell
{
    private const float spellCooldown = 30.0f;
    private const float spellManaCost = 200.0f;
    private const float range = 5.0f;
    private const float force = 100.0f;
    private const float delay = 0.3f;

    public SpellRepel() : base(spellCooldown, spellManaCost)
    {
    }

    protected override bool Execute(BallController ball)
    {
        ball.Repel(range, force, delay);
        return true;
    }
}
