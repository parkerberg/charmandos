namespace Opsive.UltimateCharacterController.Camera
{
    using Opsive.Shared.Game;
    using Opsive.Shared.Input;
    using Opsive.Shared.Utility;
    using Opsive.UltimateCharacterController.Camera.ViewTypes;
    using Opsive.UltimateCharacterController.Motion;
    using Opsive.UltimateCharacterController.Utility;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
 public CinemachineBrain cine;
 public CinemachineVirtualCamera VirtualCamera;
 private CinemachineTransposer virtTrans;

public GameObject main;
private CameraController camera;
private int rotatePress = 0;
private int rotateMax = 4;
IEnumerator Start()
{
    yield return null;
    VirtualCamera = cine.ActiveVirtualCamera as CinemachineVirtualCamera;
    virtTrans = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    camera = main.GetComponent<CameraController>();


}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){
            rotatePress = (rotateMax + rotatePress + 1) % rotateMax;
            RotateTheCamera();
            
            
            //Vector3 rotationToAdd = new Vector3(0, 90, 0); 
            //transform.Rotate(rotationToAdd);

            //transform.Rotate(0, 90, 0);
            //virtTrans.m_FollowOffset = new Vector3(12,12,0);

        }else if(Input.GetKeyDown(KeyCode.P)){
            rotatePress = (rotateMax + rotatePress - 1) % rotateMax;
            RotateTheCamera();
            //Vector3 rotationToAdd = new Vector3(0, 90, 0); 
            //transform.Rotate(rotationToAdd);

            //transform.Rotate(0, 90, 0);
           // virtTrans.m_FollowOffset = new Vector3(12,12,0);

        }
    }
    public void RotateTheCamera(){

        switch(rotatePress) 
{
  case 0:
    virtTrans.m_FollowOffset = new Vector3(0,12,-12);
    transform.rotation = Quaternion.Euler(45,0,0); 
    camera.m_ActiveViewType.AddPositionalForce(new Vector3(0,0,1), rotatePress);
    break;
  case 1:
    virtTrans.m_FollowOffset = new Vector3(-12,12,0);
    transform.rotation = Quaternion.Euler(45,90,0); 
    camera.m_ActiveViewType.AddPositionalForce(new Vector3(1,0,0), rotatePress);
    break;
  case 2:
    virtTrans.m_FollowOffset = new Vector3(0,12,12);
    transform.rotation = Quaternion.Euler(45,180,0); 
    camera.m_ActiveViewType.AddPositionalForce(new Vector3(0,0,-1), rotatePress);
    break;
  case 3:
    virtTrans.m_FollowOffset = new Vector3(12,12,0);
    transform.rotation = Quaternion.Euler(45,-90,0); 
    camera.m_ActiveViewType.AddPositionalForce(new Vector3(-1,0,0), rotatePress);
    break;
    default:
    // code block
    break;
}
    }
}
}
