using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Area : MonoBehaviour
{
    public float radius = 5.0f;
    protected GameObject player;

    protected virtual void Start()
    {
        player = PlayerStatus.instance.gameObject;
    }
    public bool WithinBounds()
    {
        if (player != null)
        {
            return (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), 
                new Vector2(transform.position.x,transform.position.y)) <= radius);
        }
        else
        {
            Debug.Log("Null Player");
            return false;
        }
    }
    protected void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, radius);
        #endif
    }
}
