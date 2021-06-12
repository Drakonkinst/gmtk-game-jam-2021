using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class GenerateChain : MonoBehaviour
{
    public GameObject ballObject;
    public GameObject chainObject;
    [Range(1, 20)]
    public int chainLength = 1;

    private Transform anchorTransform;
    private Rigidbody2D ballRB;

    private void Awake()
    {
        float linkSeparation = chainObject.GetComponent<Renderer>().bounds.extents.y * 2.0f;
        float ballRadius = ballObject.GetComponent<Renderer>().bounds.extents.y;
        float ballSeparation = ballRadius + linkSeparation / 2.0f;

        Debug.Log(linkSeparation + ", " + ballRadius);

        anchorTransform = transform;
        Transform prevTransform = anchorTransform;
        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
        Vector2 currPos = anchorTransform.position;

        // Add chain links
        for (int i = 0; i < chainLength - 1; ++i)
        {
            currPos -= new Vector2(0, linkSeparation);
            GameObject chainLink = Instantiate(chainObject, anchorTransform);
            chainLink.transform.position = currPos;
            joint.connectedBody = chainLink.GetComponent<Rigidbody2D>();

            joint = chainLink.GetComponent<DistanceJoint2D>();
            prevTransform = chainLink.transform;
        }

        // Add ball
        currPos -= new Vector2(0, ballSeparation);
        GameObject ballWeight = Instantiate(ballObject, anchorTransform);
        ballWeight.transform.position = currPos;
        joint.connectedBody = ballWeight.GetComponent<Rigidbody2D>();
        joint.connectedAnchor = Vector2.zero;
        ballRB = ballWeight.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //ballRB.AddForce(new Vector2(500.0f, 0));
    }


}
