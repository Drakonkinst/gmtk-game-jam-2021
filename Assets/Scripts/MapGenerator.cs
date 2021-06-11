using System.Collections;

using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject platform; // Default platform object
    
    public float cellWidth;
    public float cellHeight;
    public float xMin = -3.0f;
    public float xMax = 100.0f;
    public float yMin = 0.0f;
    public float yMax = 10.0f;
    private GameObject platforms; // Stores all platforms
    // (xMin,yMin) is the bottom leftmost platform at the start
    void Start()
    {
        platforms = new GameObject("Platforms");
        for(int i = 0; i < xMax; ++i)
        {
            Instantiate(platform,new Vector2(xMin + (i * cellWidth),yMin), Quaternion.identity, platforms.transform); // Create Floor
            Instantiate(platform, new Vector2(xMin + (i * cellWidth), yMax - cellHeight), Quaternion.identity, platforms.transform); ; // Create Floor
        }
        for(int j = 1; j < yMax + 2; ++j)
        {
            Instantiate(platform, new Vector2(xMin, yMin + (j * cellHeight)), Quaternion.identity, platforms.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
