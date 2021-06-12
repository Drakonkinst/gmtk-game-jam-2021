using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerController player;
    private PlayerStatus playerStatus;

    [HideInInspector]
    public BallController ball;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ball = FindObjectOfType<BallController>();
        playerStatus = PlayerStatus.instance;
    }

    private void Update()
    {
        player.ResetPlayerMovement();
        if (Input.GetKey(KeyCode.LeftShift) && player.IsGrounded() && playerStatus.mana > 0.0f)
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

        // TODO: Cannot jump if weighed down by ball--this might already be accomplished?
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
