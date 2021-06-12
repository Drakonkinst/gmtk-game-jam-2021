using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundable : MonoBehaviour
{
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
                if (hit.collider != null && hit.collider.tag == "Platform")
                {
                    hitGround = true;
                }
            }

            return hitGround;
        }

        // undefined
        return false;
        
    }


}
