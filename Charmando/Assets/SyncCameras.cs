using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCameras : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform virtualCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = virtualCamera.position;
        transform.rotation = virtualCamera.rotation;
    }
}
