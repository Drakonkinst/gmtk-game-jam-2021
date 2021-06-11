using System;

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

    void Update()
    {
        xIn = Input.GetAxis("Horizontal");
        //yIn = Input.GetAxis("Vertical");
        rb.AddForce(new Vector2(xIn * moveSpeed * Time.smoothDeltaTime,0.0f),ForceMode2D.Impulse);

        time = time + Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && time >= nextJump)
        {
            nextJump = time + jumpCooldown;
            rb.AddForce(new Vector2(0.0f, jumpForce),ForceMode2D.Impulse);
        }
    }
}
