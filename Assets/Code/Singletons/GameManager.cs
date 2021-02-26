using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance = null;

    public SpawnPoint playerSpawnPoint;

    public CameraManager cameraManager;

    void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        setupScene();
    }

    void setupScene()
    {
        SpawnPlayer();



    }

    public void SpawnPlayer()
    {
        if(playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();



            cameraManager.virtualCamera.Follow = player.transform;

        }



    }

}
