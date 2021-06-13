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
    public float delayToStartHealthRegen = 5.0f;
    public float manaRegenPerTick = 1.0f;
    public float healthRegenPerTick = 0.2f;
    public float updateTick = 0.01f;
    public bool infiniteMana = false;

    private float maxHealth;
    private float maxMana;
    private float nextManaRegenEnable = -1.0f;
    private float nextHealthRegenEnable = -1.0f;

    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        chainLength = GetComponent<GenerateChain>().chainLength;
        StartCoroutine(DoUpdate());
    }

    public void SetState(State state)
    {
        if(currentState == state)
        {
            return;
        }

        /*
        if(state == State.MovingBall)
        {
            Debug.Log(currentState.ToString());
        }//*/
        currentState = state;
        // TODO: set sprite as necessary

        if(currentState == State.Idle)
        {
            animator.SetTrigger("Idle");
        }
        else if(currentState == State.Jumping)
        {
            animator.SetTrigger("Idle");
        }
        else if(currentState == State.Moving)
        {
            animator.SetTrigger("Running");
        }
        else if(currentState == State.MovingBall)
        {
            animator.SetTrigger("Casting");
        }
    }

    public void Damage(float amount)
    {
        if(amount > healthBarShakeThreshold)
        {
            HUD.instance.healthBarDisplay.Shake();
        }
        Heal(-amount);
        nextHealthRegenEnable = Time.time + delayToStartHealthRegen;
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
            // Reload Scene
            Debug.Log("RELOAD SCENE!");
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

    private IEnumerator DoUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(updateTick);

            // Drain mana if moving ball
            if (currentState == State.MovingBall)
            {
                RemoveMana("MovingBall");
            }

            // Regenerate mana
            float currTime = Time.time;
            if (currTime > nextManaRegenEnable)
            {
                AddMana(manaRegenPerTick);
            }

            // Regenerate health
            if(currTime > nextHealthRegenEnable)
            {
                Heal(healthRegenPerTick);
            }

            // Kill player upon reaching a certain height
            if(instance.gameObject.transform.position.y <= -50.0f)
            {
                SetHealth(0);
            }
        }
    }

    public Vector2 GetPos()
    {
        return InputManager.instance.player.GetPos();
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if(chainLength < 0)
        {
            return;
        }

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, chainLength);
        #endif
    }

}
