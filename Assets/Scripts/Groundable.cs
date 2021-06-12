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

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        isCircle = circle != null;
        if (isCircle)
        {
            radius = circle.bounds.extents.y;
        }
    }

    public bool IsGrounded()
    {
        if (isCircle)
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y - radius);
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, Vector2.down, groundThreshold);

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
        if (isCircle)
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
