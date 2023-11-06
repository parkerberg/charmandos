namespace Opsive.UltimateCharacterController.ThirdPersonController.Camera.ViewTypes
{
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
    using Opsive.Shared.Input;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pointer;

public GameObject m_Character;
public Camera main;
public Camera seconmd;
public float maxRadius;
        public LayerMask layerMask;
    public Transform virtualCamera;

 void Start()
{
  
}

    // Update is called once per frame
    void Update()
    {
           main = main.GetComponent<Camera>();
        
                Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = new Ray();
        ray.origin = virtualCamera.position;
        ray.direction = main.ScreenPointToRay(Input.mousePosition).direction;

//Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 9999f, layerMask)){
           // Debug.Log(raycastHit.point);
            pointer.position = raycastHit.point;
           // pointer.Rotate(Vector3.forward * 200 * Time.deltaTime);

        }

        Ray ray3 = seconmd.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray3, out RaycastHit raycastHit2, 999f, layerMask))
        {
            Vector3 mousePos = raycastHit2.point;
 
            Vector3 difference = mousePos - m_Character.transform.position;
            float magnitude = difference.magnitude;
            if (magnitude > maxRadius) {
                difference = difference * (maxRadius / magnitude);
            }
            transform.position = m_Character.transform.position + difference;      
        }



                        Ray ray2 = new Ray();
        ray2 = seconmd.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray2.origin, ray2.direction * 1000, Color.red);
    }
}
}
