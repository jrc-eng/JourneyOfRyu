using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager sharedInstance = null;

    
    public CinemachineVirtualCamera virtualCamera;



    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");

        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
