using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public float triggerDistance = 10.0f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position,transform.position) <= triggerDistance)
        {
            WorldManager.game.SetPlayerSpawn(new Vector2(transform.position.x, transform.position.y));
            Destroy(this);
        }
    }
}
