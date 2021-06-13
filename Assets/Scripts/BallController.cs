using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Groundable
{
    public enum State
    {
        Unpowered,
        Controlled,
        Hovering
    }

    private const float chainLengthBuffer = 0.5f;
    private const float distanceSlowingThreshold = 0.9f;
    private const float maxSlowing = 0.8f;
    private const float burningTick = 0.5f;
    private const float burningThreshold = 0.1f;

    public float maxSpeed = 10.0f;
    public float velocityThresholdForPlayerDamage = 8.0f;
    public float velocityThresholdForEnemyDamage = 5.0f;
    public float enemyDamage = 0.5f;

    [HideInInspector]
    public State currentState = State.Unpowered;
    
    [HideInInspector]
    public float ballRadius;

    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector2 ballMovementDirection = Vector2.zero;

    private State lastStateBeforeControlled = State.Unpowered;

    private bool lastOnGround = false;
    private bool canHover = false;
    private bool isBurning = false;
    private float burningDamageToPlayer;
    private float burningDamageToEnemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
        ballRadius = GetComponent<Collider2D>().bounds.extents.y;
        StartCoroutine(DoBurning());
    }

    private void FixedUpdate()
    {
        float chainLength = PlayerStatus.instance.chainLength - chainLengthBuffer;
        Vector2 playerPos = PlayerStatus.instance.GetPos();
        Vector2 ballToPlayer = PredictNextPosition(rb.velocity, Time.deltaTime) - playerPos;
        float dist = ballToPlayer.magnitude;

        if (currentState == State.Controlled)
        {
            lastOnGround = false;
            //rb.bodyType = RigidbodyType2D.Dynamic;
            float distRatio = Mathf.Clamp01(dist / chainLength);
            float currentSpeed = maxSpeed;

            // Not sure if this actually does anything tbh
            if (distRatio > distanceSlowingThreshold)
            {
                float speedMultiplier = 1.0f - ((distRatio - distanceSlowingThreshold) / (1.0f - distanceSlowingThreshold)) * maxSlowing;
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
        else if(currentState == State.Hovering)
        {

        }
        else if(currentState == State.Unpowered)
        {
            bool onGround = IsGrounded();
            if(onGround != lastOnGround)
            {
                lastOnGround = onGround;
                if(onGround)
                {
                    CameraController.instance.GetComponent<CameraShake>().Shake();
                }
            }

            if(IsOnFlatGround())
            {
                rb.bodyType = RigidbodyType2D.Static;
            } else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        if(IsOnFlatGround())
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        bool isGround = IsGround(col.collider);
        bool onGround = IsGrounded();
        if (isGround)
        {
            if(currentState == State.Hovering)
            {
                return;
            }
            CameraController.instance.GetComponent<CameraShake>().Shake();
        }
        else if(currentState == State.Unpowered && !onGround)
        {
            bool shouldDamagePlayer = col.relativeVelocity.sqrMagnitude >= velocityThresholdForPlayerDamage * velocityThresholdForPlayerDamage;
            bool shouldDamageEnemy = col.relativeVelocity.sqrMagnitude >= velocityThresholdForEnemyDamage * velocityThresholdForEnemyDamage;
            //Debug.Log(col.relativeVelocity.magnitude);
            if(col.collider.tag == "Player")
            {
                if(shouldDamagePlayer)
                {
                    PlayerStatus.instance.Damage(30.0f);
                }
            } else if(col.collider.tag == "Enemy")
            {
                if(shouldDamagePlayer)
                {
                    // Enough damage to kill it
                    col.collider.GetComponent<EnemyController>().destruct();
                    rb.AddForce(new Vector2(0.0f, -50.0f));
                    rb.velocity = new Vector2(0.0f, -25.0f);
                } else if(shouldDamageEnemy)
                {
                    col.collider.GetComponent<EnemyController>().receiveDamage(enemyDamage);
                }
            }
        }
    }

    public void SetState(State state)
    {
        if(state == currentState)
        {
            return;
        }


        Collider2D myCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = PlayerStatus.instance.GetComponent<Collider2D>();

        // "Enter state" events
        if (state == State.Controlled)
        {
            rb.gravityScale = 0.0f;
            Physics2D.IgnoreCollision(myCollider, playerCollider, true);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else if(state == State.Hovering)
        {
            rb.gravityScale = 0.0f;
            Physics2D.IgnoreCollision(myCollider, playerCollider, false);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
        else if(state == State.Unpowered)
        {
            rb.gravityScale = 1.0f;
            Physics2D.IgnoreCollision(myCollider, playerCollider, false);
            rb.velocity = Vector2.zero;
        }

        currentState = state;

        //Debug.Log("Set state to " + currentState.ToString());
    }

    public void SetPoweredState(bool flag)
    {
        if (flag && currentState != State.Controlled)
        {
            //Debug.Log("Saved last state as " + currentState.ToString());
            lastStateBeforeControlled = currentState;
            SetState(State.Controlled);
        }
        else if (!flag && currentState == State.Controlled)
        {
            if(lastStateBeforeControlled == State.Hovering && !canHover)
            {
                SetState(State.Unpowered);
            }
            else
            {
                SetState(lastStateBeforeControlled);
            }
            //Debug.Log("Restoring state as " + currentState.ToString());
        }
    }

    public void SetHoveringFor(float duration)
    {
        canHover = true;
        StartCoroutine(StopHovering(duration));
    }

    private IEnumerator StopHovering(float duration)
    {
        yield return new WaitForSeconds(duration);
        canHover = false;
        //Debug.Log(currentState.ToString());
        if (currentState != BallController.State.Controlled)
        {
            SetState(BallController.State.Unpowered);
        }
    }

    public void SetBurningFor(float duration, float damageToPlayer, float damageToEnemy)
    {
        isBurning = true;
        burningDamageToPlayer = damageToPlayer;
        burningDamageToEnemy = damageToEnemy;
        StartCoroutine(StopBurning(duration));
    }

    private IEnumerator StopBurning(float duration)
    {
        yield return new WaitForSeconds(duration);
        isBurning = false;
    }

    private IEnumerator DoBurning()
    {
        while(true)
        {
            yield return new WaitForSeconds(burningTick);
            if(isBurning)
            {
                List<Collider2D> touching = new List<Collider2D>();
                int numFound = Physics2D.OverlapCircle(myTransform.position, ballRadius + burningThreshold, cf.NoFilter(), touching);

                foreach(Collider2D hit in touching)
                {
                    if(hit.tag == "Player")
                    {
                        PlayerStatus.instance.Damage(burningDamageToPlayer);
                    }
                    else if(hit.tag == "Enemy")
                    {
                        hit.GetComponent<EnemyController>().receiveDamage(burningDamageToEnemy);
                    }
                    Debug.Log("Damaged " + hit.tag);
                }
            }
        }
    }

    public void Attract(float range, float force, float delay)
    {
        StartCoroutine(DoAttract(range, force, delay));
    }

    private IEnumerator DoAttract(float range, float force, float delay)
    {
        yield return new WaitForSeconds(delay);
        List<Collider2D> touching = new List<Collider2D>();
        Vector2 pos = myTransform.position;
        int numFound = Physics2D.OverlapCircle(pos, ballRadius + range, cf.NoFilter(), touching);

        foreach (Collider2D hit in touching)
        {
            if (hit.tag == "Player" || hit.tag == "Enemy")
            {
                Vector2 forceDir = (pos - (Vector2)hit.transform.position);
                hit.GetComponent<Rigidbody2D>().AddForce(forceDir.normalized * force + Vector2.up * (force / 5.0f), ForceMode2D.Impulse);
            }
        }
    }

    public void Repel(float range, float force, float delay)
    {
        StartCoroutine(DoRepel(range, force, delay));
    }

    private IEnumerator DoRepel(float range, float force, float delay)
    {
        yield return new WaitForSeconds(delay);
        List<Collider2D> touching = new List<Collider2D>();
        Vector2 pos = myTransform.position;
        int numFound = Physics2D.OverlapCircle(pos, ballRadius + range, cf.NoFilter(), touching);

        foreach (Collider2D hit in touching)
        {
            if (hit.tag == "Player" || hit.tag == "Enemy")
            {
                Vector2 forceDir = ((Vector2)hit.transform.position - pos);
                hit.GetComponent<Rigidbody2D>().AddForce(forceDir.normalized * force + Vector2.up * (force / 5.0f), ForceMode2D.Impulse);
            }
        }
    }

    public void ShootBall(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
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
        if (myTransform == null || currentState != State.Controlled)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(myTransform.position, PredictNextPosition(rb.velocity, 0.1f));
    }
    
    public bool IsOnBall(Vector2 pos)
    {
        return (pos - (Vector2)myTransform.position).sqrMagnitude <= ballRadius * ballRadius;
    }
}
