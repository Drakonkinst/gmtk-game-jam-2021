using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public float breakForce = 25.0f;
    
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided");
        if(col.collider.gameObject.tag == "Ball" && col.relativeVelocity.magnitude >= breakForce)
        {
            //Trigger Breaking Sound
            Debug.Log("Velocity Received: " + col.relativeVelocity.magnitude);
            Destroy(gameObject);
        }
    }
}
