using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public bool isUnlocked = false;
    public float cooldown;
    public float manaCost;

    protected ContactFilter2D cf;
    private float nextEnable = 0.0f;

    public Spell(float cooldown, float manaCost)
    {
        this.cooldown = cooldown;
        this.manaCost = manaCost;
    }

    public void Cast(BallController ball)
    {
        if (OnCooldown())
        {
            SetResultText("Spell is on cooldown!");
            return;
        }

        if(PlayerStatus.instance.mana < manaCost)
        {
            SetResultText("Not enough mana!");
            return;
        }

        float currentTime = Time.time;
        if (currentTime >= nextEnable)
        {
            Execute(ball);
            PlayerStatus.instance.RemoveMana(manaCost);
            nextEnable = currentTime + cooldown;
        }
    }

    protected abstract void Execute(BallController ball);

    protected void SetResultText(string text)
    {
        HUD.instance.SetSpellResultText(text);
    }

    public Spell Unlock()
    {
        isUnlocked = true;
        return this;
    }

    public bool OnCooldown()
    {
        return Time.time < nextEnable;
    }
}
