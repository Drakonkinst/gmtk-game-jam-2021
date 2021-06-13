using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerController player;

    [HideInInspector]
    public Spell[] spells = new Spell[6];

    [HideInInspector]
    public BallController ball;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ball = FindObjectOfType<BallController>();
        playerStatus = PlayerStatus.instance;

        spells[0] = new SpellHover().Unlock();
        spells[1] = new SpellShoot().Unlock();
        spells[2] = new SpellEnflame().Unlock();
        spells[3] = new SpellAttract().Unlock();
        spells[4] = new SpellRepel().Unlock();

        // Attach to icons
        HUD.instance.SetSpells(spells);
        HUD.instance.UpdateSpellsUnlocked();
    }

    private void Update()
    {
        player.ResetPlayerMovement();
        bool pressingShift = Input.GetKey(KeyCode.LeftShift);
        bool isGrounded = player.IsGrounded();
        bool enoughMana = playerStatus.mana > 0.0f;

        if(pressingShift && !isGrounded)
        {
            HUD.instance.SetSpellResultText("Must be on the ground!");
        }
        else if(pressingShift && !enoughMana)
        {
            HUD.instance.SetSpellResultText("Not enough mana!");
        }

        if (pressingShift && isGrounded && enoughMana)
        {
            // temp
            CameraController.instance.SetTargetToBall();
            ball.SetPoweredState(true);
            player.SetBallState(true);
            playerStatus.SetState(PlayerStatus.State.MovingBall);

            DoBallInput();
        }
        else
        {
            // temp
            CameraController.instance.SetTargetToPlayer();
            ball.SetPoweredState(false);
            player.SetBallState(false);

            DoPlayerInput();
        }
        DoAbilityInput();
    }

    private void DoPlayerInput()
    {

        if (IsPressingLeft())
        {
            player.MoveLeft();
        }

        if (IsPressingRight())
        {
            player.MoveRight();
        }

        bool isGrounded = player.IsGrounded();
        if(PressedUp() && isGrounded)
        {
            player.Jump();
        }

    }

    private void DoBallInput()
    {
        ball.ResetBallMovement();
        if(IsPressingDown())
        {
            ball.MoveDown();
        }

        if(IsPressingUp())
        {
            ball.MoveUp();
        }

        if(IsPressingLeft())
        {
            ball.MoveLeft();
        }

        if(IsPressingRight())
        {
            ball.MoveRight();
        }
    }

    private void DoAbilityInput()
    {
        for(int i = 0; i < spells.Length; ++i)
        {
            if(Input.GetKeyDown("" + (i + 1)))
            {
                Spell spell = spells[i];
                if (spell != null && spell.isUnlocked)
                {
                    spell.Cast(ball);
                    Debug.Log("Attempting to cast spell " + (i + 1));
                }
            }
        }
    }

    private bool IsPressingLeft()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a");
    }

    private bool IsPressingRight()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d");
    }

    private bool IsPressingUp()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w") || Input.GetKey(KeyCode.Space);
    }

    private bool IsPressingDown()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s");
    }

    public bool PressedUp()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.Space);
    }


}
