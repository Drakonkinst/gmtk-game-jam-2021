using System;
using System.Collections;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Vector2 lastSpawnPoint;
    public GameObject melee;
    public GameObject ranged;
    public GameObject player;
    public float spawningTimer = 10.0f;
    private float nextSpawn;
    public Vector2[] spawns;
    private float time = 0.0f;
    void start()
    {
        spawns = new Vector2[] { new Vector2(0.0f,0.0f), new Vector2(-10.0f,-5.0f), new Vector2(10.0f,-5.0f) };
        nextSpawn = spawningTimer;
    }

    void update()
    {
        time = time + Time.deltaTime;
        if(time > nextSpawn)
        {
            int pos = (int) ((UnityEngine.Random.value * 3) - 1);
            int type = (int) ((UnityEngine.Random.value * 2) - 1);
            SpawnEnemy(spawns[pos], type);
            nextSpawn = time + spawningTimer;
        }
    }

    public void SpawnEnemy(Vector2 pos, int type)
    {
        GameObject enemyType = (type == 0 ? melee : ranged);
        Vector3 finalPosition = new Vector3(transform.position.x + pos.x, transform.position.y + pos.y, 0.0f);
        Debug.Log("SPAWNING");
        Instantiate(enemyType, finalPosition, Quaternion.identity);
    }
}
