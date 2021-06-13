using System;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : Groundable
{
    public float moveSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 10.0f;
    public float responsiveness = 1.0f;

    public float ballStateMass = 20.0f;

    public AudioClip jumpSound;

    [HideInInspector]
    public float playerXMovement = 0.0f;

    private Rigidbody2D rb;
    private float xIn, yIn;
    private float nextJump;
    private float defaultMass = 1.0f;
    private Transform myTransform;
    private PlayerStatus playerStatus;
    private float prevXMovement = 0.0f;
    private SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();
        myTransform = transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStatus = PlayerStatus.instance;
        defaultMass = rb.mass;
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(playerXMovement, 0.0f);
        Vector2 currentVelocity = new Vector2(prevXMovement, 0.0f);
        rb.AddForce((targetVelocity - new Vector2(rb.velocity.x, 0.0f)) * responsiveness);

        // TODO: Can't jump if held down by ball--this might be done for us
        if(playerXMovement > 0)
        {
            sprite.flipX = false;
        }
        if(playerXMovement < 0)
        {
            sprite.flipX = true;
        }

        bool isGrounded = IsGrounded();

        if(InputManager.instance.ball.currentState == BallController.State.Controlled)
        {
            return;
        }
        if (isGrounded) 
        {
            if (Mathf.Approximately(playerXMovement, 0.0f))
            {
                playerStatus.SetState(PlayerStatus.State.Idle);
            }
            else
            {
                playerStatus.SetState(PlayerStatus.State.Moving);
            }
        }
        else
        {
            playerStatus.SetState(PlayerStatus.State.Jumping);
        }

        /*
        // OnGround indicator
        if (isGrounded)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }*/
}

    public void SetBallState(bool flag)
    {
        if(flag)
        {
            rb.mass = ballStateMass;
        } 
        else
        {
            rb.mass = defaultMass;
        }
    }

    public void MoveLeft()
    {
        playerXMovement -= moveSpeed;
    }

    public void MoveRight()
    {
        playerXMovement += moveSpeed;
    }

    public void ResetPlayerMovement()
    {
        playerXMovement = 0.0f;
    }

    public void Jump()
    {
        SoundManager.Instance.Play(jumpSound, myTransform, 0.5f, 1.2f);
        rb.AddForce(new Vector2(0.0f, jumpForce));
    }

    public Vector2 GetPos()
    {
        return myTransform.position;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

}
