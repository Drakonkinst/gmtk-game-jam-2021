using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleporter : Area
{
    public int scene = 0; 

    // Update is called once per frame
    void Update()
    {
        if(WithinBounds())
        {
            //Transition to new scene based on "scene" variable
        }
    }
}
