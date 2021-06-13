using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : Area
{
    public int scene = 0; // 
    private string scenePath;
    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
        string basePath = "./Assets/Main Game/Scenes/";
        scenePath = basePath + scene;
    }

    void FixedUpdate()
    {
        if(base.WithinBounds())
        {
            Debug.Log("Player within Bounds");
            SceneManager.LoadScene(1);
        }
    }
}
