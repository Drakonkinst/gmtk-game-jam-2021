using System;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float groundThreshold = 0.1f;

    public float moveSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 10.0f;
    
    public float ballStateMass = 20.0f;

    private Rigidbody2D rb;
    private float xIn, yIn;
    private float nextJump;
    private float time = 0.0f;
    private float playerXMovement = 0.0f;
    private float defaultMass = 1.0f;
    private Transform myTransform;
    private float radius;

    private void Awake()
    {
        myTransform = transform;
        radius = GetComponent<Collider2D>().bounds.extents.y;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultMass = rb.mass;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(playerXMovement, rb.velocity.y);
        time = time + Time.deltaTime;

        /*// TODO: Can't jump if held down by ball
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            nextJump = time + jumpCooldown;
            //rb.velocity += new Vector2(0.0f, jumpForce);
            Jump();
        }

        //Debug.Log(IsGrounded());*/
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
        rb.AddForce(new Vector2(0.0f, jumpForce));
    }

    public Vector2 GetPos()
    {
        return myTransform.position;
    }

    public bool IsGrounded()
    {
        Debug.DrawRay(myTransform.position, Vector2.down * (radius + groundThreshold), Color.yellow, 10.0f);
        RaycastHit2D hit = Physics2D.Raycast(myTransform.position, Vector2.down, radius + groundThreshold);

        if(hit.collider != null && hit.collider.tag == "Platform")
        {
            return true;
        }
        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.tag);
        }
        return false;
    }

}
