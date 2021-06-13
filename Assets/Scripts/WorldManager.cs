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

    void Start()
    {
        lastSpawnPoint = new Vector2(0.0f, -3.48f);
        SpawnPlayer();
    }

    public void SetPlayerSpawn(Vector2 spawn)
    {
        lastSpawnPoint = spawn;
    }

    public void SpawnPlayer()
    {
        if(currentPlayer != null) {
            DestroyChain();
            Destroy(currentPlayer);
        }
        Vector3 spawnLocation = new Vector3(lastSpawnPoint.x, lastSpawnPoint.y, 0.0f);
        currentPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        
    }
    public void DestroyChain()
    {
        foreach (Transform child in currentPlayer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
