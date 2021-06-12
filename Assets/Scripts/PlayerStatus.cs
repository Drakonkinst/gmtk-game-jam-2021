using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    public float health = 100.0f;
    public float mana = 100.0f;

    [HideInInspector]
    public float chainLength = -1.0f;

    [HideInInspector]

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        chainLength = GetComponent<GenerateChain>().chainLength;
    }

    public Vector2 GetPos()
    {
        return InputManager.instance.player.GetPos();
    }

    private void OnDrawGizmos()
    {
        if(chainLength < 0)
        {
            return;
        }

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, chainLength);
    }

}
