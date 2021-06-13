using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    public enum State
    {
        Idle,
        Moving,
        Jumping,
        MovingBall
    }

    [HideInInspector]
    public State currentState = State.Idle;

    public float health = 100.0f;
    public float mana = 300.0f;
    public float healthBarShakeThreshold = 20.0f;

    public float delayToStartManaRegen = 1.5f;
    public float manaRegenPerTick = 1.0f;
    public float manaTick = 0.01f;
    public float updateTick = 0.01f;
    public bool infiniteMana = false;

    private float maxHealth;
    private float maxMana;
    private float nextManaRegenEnable = -1.0f;

    [HideInInspector]
    public float chainLength = -1.0f;

    private Dictionary<string, float> eventToDamageValue = new Dictionary<string, float>()
    {
        { "MovingBall", 1.0f }
    };

    private void Awake()
    {
        instance = this;
        maxHealth = health;
        maxMana = mana;
    }

    private void Start()
    {
        spawnPoint = new Vector2(0.0f, -3.48f);
        chainLength = GetComponent<GenerateChain>().chainLength;
        StartCoroutine(DoUpdate());
        StartCoroutine(RegenerateMana());
    }

    public void SetState(State state)
    {
        currentState = state;
        // TODO: set sprite as necessary
    }

    public void Damage(float amount)
    {
        if(amount > healthBarShakeThreshold)
        {
            HUD.instance.healthBarDisplay.Shake();
        }
        Heal(-amount);
    }

    public void Heal(float amount)
    {
        SetHealth(health + amount);
    }

    public void SetHealth(float amount)
    {
        health = Mathf.Clamp(amount, 0.0f, maxHealth);
        if(Mathf.Approximately(health, 0.0f))
        {
            WorldManager.game.SpawnPlayer(); // Will destroy the chain and ball first, then the last player, then spawn a new player at the last checkpoint
        }
        HUD.instance.healthBarDisplay.SetPercent(health / maxHealth);
    }

    public void AddMana(float amount)
    {
        SetMana(mana + amount);
    }

    public void RemoveMana(float amount)
    {
        if(infiniteMana)
        {
            return;
        }

        AddMana(-amount);
        nextManaRegenEnable = Time.time + delayToStartManaRegen;
    }

    public void RemoveMana(string eventString)
    {
        if(!eventToDamageValue.ContainsKey(eventString))
        {
            Debug.Log("Invalid event: " + eventString);
            return;
        }

        RemoveMana(eventToDamageValue[eventString]);
    }

    public void SetMana(float amount)
    {
        mana = Mathf.Clamp(amount, 0.0f, maxMana);
        HUD.instance.manaBarDisplay.SetPercent(mana / maxMana);
    }
    public void SetSpawn(Vector2 spawn)
    {
        spawnPoint = spawn;
    }

    private IEnumerator RegenerateMana()
    {
        while(true)
        {
            yield return new WaitForSeconds(manaTick);

            float currTime = Time.time;
            if(currTime > nextManaRegenEnable)
            {
                AddMana(manaRegenPerTick);
            }
        }
    }

    private IEnumerator DoUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(updateTick);

            if (currentState == State.MovingBall)
            {
                RemoveMana("MovingBall");
            }
        }
    }

    public Vector2 GetPos()
    {
        return InputManager.instance.player.GetPos();
    }

    private void OnDrawGizmos()
    {
        if(chainLength < 0)
        {
            return;
        }

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, chainLength);
    }

}
