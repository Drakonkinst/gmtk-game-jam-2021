using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class GenerateChain : MonoBehaviour
{
    public GameObject ballObject;
    public GameObject chainObject;
    [Range(1, 20)]
    public int numSegments = 1;

    [HideInInspector]
    public float chainLength;

    private Transform anchorTransform;
    private Rigidbody2D ballRB;

    private void Awake()
    {
        float linkSeparation = chainObject.GetComponent<Renderer>().bounds.extents.x * 2.0f;
        float ballRadius = ballObject.GetComponent<Renderer>().bounds.extents.x;
        float ballSeparation = ballRadius + linkSeparation / 2.0f;

        anchorTransform = transform;
        Transform prevTransform = anchorTransform;
        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
        Vector2 currPos = anchorTransform.position;

        // Add chain links
        for (int i = 0; i < numSegments - 1; ++i)
        {
            currPos -= new Vector2(linkSeparation, 0);
            GameObject chainLink = Instantiate(chainObject, anchorTransform);
            chainLink.transform.position = currPos;
            joint.connectedBody = chainLink.GetComponent<Rigidbody2D>();

            joint = chainLink.GetComponent<DistanceJoint2D>();
            prevTransform = chainLink.transform;
        }

        // Add ball
        currPos -= new Vector2(ballSeparation, 0.0f);
        GameObject ballWeight = Instantiate(ballObject, anchorTransform);
        ballWeight.transform.position = currPos;
        joint.connectedBody = ballWeight.GetComponent<Rigidbody2D>();
        joint.connectedAnchor = Vector2.zero;
        ballWeight.GetComponent<Collider2D>().tag = "Ball";
        ballRB = ballWeight.GetComponent<Rigidbody2D>();

        //Debug.Log(linkSeparation + ", " + ballRadius);
        chainLength = (linkSeparation * numSegments + ballRadius) * 2.0f / 1.2f;
        //Debug.Log(chainLength);
    }

    private void Start()
    {
        foreach(DistanceJoint2D joint in GetComponentsInChildren<DistanceJoint2D>()) {
            joint.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 0.0f));
            //Debug.Log("Found");
            //Debug.Log(joint.distance);
            //joint.autoConfigureDistance = false;
        }
    }
}
