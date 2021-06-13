using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHover : Spell
{
    private const float spellCooldown = 5.0f;
    private const float spellManaCost = 100.0f;

    public SpellHover() : base(spellCooldown, spellManaCost)
    {
        
    }

    protected override bool Execute(BallController ball)
    {
        ball.SetState(BallController.State.Hovering);
        // duration == cooldown
        ball.SetHoveringFor(spellCooldown);
        return true;
    }
}
