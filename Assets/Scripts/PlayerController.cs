using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    float xIn, yIn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xIn = Input.GetAxis("Horizontal");
        yIn = Input.GetAxis("Vertical");
        transform.Translate(xIn * moveSpeed, yIn * moveSpeed, 0);
    }
}
