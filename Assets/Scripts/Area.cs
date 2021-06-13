using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Area : MonoBehaviour
{
    public float radius = 5.0f;
    private Transform player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    protected bool WithinBounds()
    {
        return (Vector3.Distance(player.position,transform.position) <= radius);
    }
    protected void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, radius);
    }
}
