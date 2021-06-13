using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager game;
    public Vector2 lastSpawnPoint;
    public GameObject playerPrefab;
    private GameObject currentPlayer;

    private void Awake()
    {
        game = this;
    }
    public void SpawnEnemy()
    {

    }


    public void DestroyChain()
    {
        foreach (Transform child in currentPlayer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
