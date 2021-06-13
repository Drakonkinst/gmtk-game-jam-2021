using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raisable : MonoBehaviour
{
    public float speed = 0.05f;
    public float delay = 2.0f;
    public float time = 0.0f;
    public float duration = 1.2f;
    
    void Update()
    {
        time = time + Time.deltaTime;
        if(time >= delay)
        {
            if(time <= duration + delay)
            {
                transform.Translate(0.0f, speed, 0.0f);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
