using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundable : MonoBehaviour
{
    private const string platformTag = "Platform";
    private const string slantedPlatformTag = "Slanted";
    private const string ballTag = "Ball";

    private const float groundThreshold = 0.1f;

    private float radius;
    private bool isCircle = false;
    private bool isCapsule = false;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        isCircle = circle != null;
        isCapsule = capsule != null;
        if (isCircle)
        {
            radius = circle.bounds.extents.y;
        }
        else if(isCapsule)
        {
            radius = capsule.bounds.extents.y;
        }
    }

    public bool IsGrounded()
    {
        if (isCircle || isCapsule)
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y - radius);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, Vector2.down, groundThreshold);
            Debug.DrawRay(transform.position, Vector2.down * (radius + groundThreshold), Color.yellow, 10.0f);

            bool hitGround = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (IsGround(hit.collider))
                {
                    hitGround = true;
                }
            }

            return hitGround;
        }

        // undefined
        return false;
    }

    public bool IsOnFlatGround()
    {
        if (isCircle || isCapsule)
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y - radius);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, Vector2.down, groundThreshold);

            bool hitGround = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (IsFlatPlatform(hit.collider))
                {
                    hitGround = true;
                }
            }

            return hitGround;
        }

        // undefined
        return false;

    }

    public bool IsGround(Collider2D collider)
    {
        bool sourceIsBall = GetComponent<Collider2D>().tag == ballTag;
        return collider != null
            && (collider.tag == platformTag
             || collider.tag == slantedPlatformTag
             || (!sourceIsBall && collider.tag == ballTag));
    }

    public bool IsFlatPlatform(Collider2D collider)
    {
        return collider != null && (collider.tag == platformTag);

    }




}
