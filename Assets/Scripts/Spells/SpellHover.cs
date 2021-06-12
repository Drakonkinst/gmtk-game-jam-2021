using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHover : Spell
{
    private const float duration = 5.0f;
    public SpellHover() : base(duration, 100.0f)
    {
        
    }

    protected override void Execute(BallController ball)
    {
        ball.SetState(BallController.State.Hovering);
        ball.SetHoveringFor(duration);
    }
}
