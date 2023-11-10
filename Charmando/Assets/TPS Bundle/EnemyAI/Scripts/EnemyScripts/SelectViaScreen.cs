using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
public class SelectViaScreen : MonoBehaviour
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
                        if(hitHover.transform.root.gameObject.GetComponent<StateController>().viewArcActive == false && gameObject.GetComponent<AiHub>().globalArcEnabled == false)
                        {
                            gameObject.GetComponent<AiHub>().globalArcEnabled = true;
                        hitHover.transform.root.gameObject.GetComponent<StateController>().viewArcActive = true;
                        hitHover.transform.root.gameObject.GetComponent<StateController>().viewArc.SetActive(true);
                        hitHover.transform.root.gameObject.GetComponent<FieldOfView>().enabled = true;
                       Debug.Log("Right Clicked Enemy active true");
                        }else if(hitHover.transform.root.gameObject.GetComponent<StateController>().viewArcActive == true && gameObject.GetComponent<AiHub>().globalArcEnabled == true)
                        {
                            gameObject.GetComponent<AiHub>().globalArcEnabled = false;
                        hitHover.transform.root.gameObject.GetComponent<StateController>().viewArcActive = false;
                        hitHover.transform.root.gameObject.GetComponent<StateController>().viewArc.SetActive(false);
                        hitHover.transform.root.gameObject.GetComponent<FieldOfView>().enabled = false;
                       Debug.Log("Right Clicked Enemy active false");
                        }
                    }
                }
            }
            
    }
}
