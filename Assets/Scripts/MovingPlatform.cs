using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MovingPlatform : Area
{
    // Update is called once per frame
    void Update()
    {
        if(base.WithinBounds())
        {
            transform.Translate(1.0f, 0.0f,0.0f);
            PlayerStatus.instance.gameObject.transform.Translate(1.0f, 0.0f, 0.0f);
        }
    }
}
