using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundable : MonoBehaviour
{
    private const string platformTag = "Platform";
    private const string slantedPlatformTag = "Slanted";
    private const string ballTag = "Ball";

    private const float groundThreshold = 0.1f;
    private const float raycastRadius = 0.1f;

    private float radius;
    private bool isCircle = false;
    private bool isCapsule = false;
    private ContactFilter2D cf;

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
        return GetGround() != null;
    }

    public Collider2D GetGround()
    {
        if (isCircle || isCapsule)
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y - radius - raycastRadius);
            List<Collider2D> hits = new List<Collider2D>();
            int numFound = Physics2D.OverlapCircle(startPos, raycastRadius, cf.NoFilter(), hits);
            Debug.DrawRay(transform.position, Vector2.down * (radius + groundThreshold), Color.yellow, 10.0f);

            foreach (Collider2D hit in hits)
            {
                if (IsGround(hit))
                {
                    return hit;
                }
            }
        }

        // undefined
        return null;
    }

    // Get bounds of current platform
    public Collider2D GetPlatform()
    {
        Collider2D col = GetGround();
        if(IsPlatform(col))
        {
            return col;
        }
        return null;
    }

    public bool IsOnFlatGround()
    {
        return IsFlatPlatform(GetGround());
    }

    public bool IsGround(Collider2D collider)
    {
        bool sourceIsBall = GetComponent<Collider2D>().tag == ballTag;
        return collider != null
            && (collider.tag == platformTag
             || collider.tag == slantedPlatformTag
             || (!sourceIsBall && collider.tag == ballTag));
    }

    public bool IsPlatform(Collider2D collider)
    {
        return collider != null && (collider.tag == platformTag || collider.tag == slantedPlatformTag);
    }

    public bool IsFlatPlatform(Collider2D collider)
    {
        return collider != null && (collider.tag == platformTag);
    }





}
