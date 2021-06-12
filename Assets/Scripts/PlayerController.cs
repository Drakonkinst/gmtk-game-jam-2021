using System;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 10.0f;

    private Rigidbody2D rb;
    private float xIn, yIn;
    private float nextJump;
    private float time = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else 
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        time = time + Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && time >= nextJump)
        {
            nextJump = time + jumpCooldown;
            rb.velocity += new Vector2(0.0f, jumpForce);
        }
    }

}
