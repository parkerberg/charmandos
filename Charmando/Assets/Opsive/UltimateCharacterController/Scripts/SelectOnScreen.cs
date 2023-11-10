using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
using UnityEngine.AI;

namespace EnemyAI
{
public class SelectOnScreen : MonoBehaviour
{
    public Camera main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(1)){
            Debug.Log("Pressed right-click.");

            Ray ray = main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitHover;
                Debug.DrawRay(main.transform.position, main.transform.forward * 1000, Color.yellow);
                if (Physics.Raycast(ray, out hitHover))
                {
 
                    Debug.Log("Right Clicked something Name: " + hitHover.transform.gameObject.name);
                    if (hitHover.transform.gameObject.layer == 12)
                    {
                       // hitHover.transform.root.gameObject.GetComponent<FieldOfView>().viewArcActive = true;
                       Debug.Log("Right Clicked Enemy");
                    }
                }
            }
            
    }
}
}
