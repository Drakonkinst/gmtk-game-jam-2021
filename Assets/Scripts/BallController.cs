using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private const float chainLengthBuffer = 0.5f;
    private const float distanceSlowingThreshold = 0.7f;
    private const float maxSlowing = 0.8f;

    public bool isPowered = false;
    public float maxSpeed = 10.0f;

    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector2 ballMovementDirection = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        if (isPowered)
        {
            float chainLength = PlayerStatus.instance.chainLength - chainLengthBuffer;
            Vector2 playerPos = PlayerStatus.instance.GetPos();
            //Vector2 ballToPlayer = GetPos() - playerPos;
            Vector2 ballToPlayer = PredictNextPosition(rb.velocity, Time.deltaTime) - playerPos;
            float dist = ballToPlayer.magnitude;
            float distRatio = Mathf.Clamp01(dist / chainLength);
            float currentSpeed = maxSpeed;

            // Not sure if this actually does anything tbh
            if(distRatio > distanceSlowingThreshold)
            {
                float speedMultiplier = 1.0f - ((distRatio - distanceSlowingThreshold) / (1.0f - distanceSlowingThreshold)) * (1.0f - maxSlowing);
                Debug.Log(speedMultiplier);
                currentSpeed *= speedMultiplier;
            }

            rb.velocity = ballMovementDirection.normalized * currentSpeed;

            // Fixed position
            if (dist > chainLength)
            {
                // Should modify velocity so it can do its own movement, do not set Transform
                Vector2 newDirection = -ballToPlayer.normalized + rb.velocity.normalized;
                rb.velocity = newDirection * currentSpeed;
            }
        }
    }

    public void SetPoweredState(bool flag)
    {
        if (flag == isPowered)
        {
            return;
        }

        isPowered = flag;
        rb.gravityScale = isPowered ? 0.0f : 1.0f;

        if (!isPowered)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private Vector2 PredictNextPosition(Vector2 velocity, float delay)
    {
        return GetPos() + velocity * delay;
    }

    public void ResetBallMovement()
    {
        ballMovementDirection = Vector2.zero;
    }

    public void MoveUp()
    {
        ballMovementDirection += Vector2.up;
    }

    public void MoveDown()
    {
        ballMovementDirection += Vector2.down;
    }

    public void MoveLeft()
    {
        ballMovementDirection += Vector2.left;
    }

    public void MoveRight()
    {
        ballMovementDirection += Vector2.right;
    }

    public Vector2 GetPos()
    {
        return myTransform.position;
    }

    private void OnDrawGizmos()
    {
        if (myTransform == null || !isPowered)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(myTransform.position, PredictNextPosition(rb.velocity, 0.1f));
    }
}
