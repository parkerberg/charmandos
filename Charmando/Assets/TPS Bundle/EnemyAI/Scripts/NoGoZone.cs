using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGoZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collision collision)
    {
        Debug.Log("Player Entered " + collision.collider.transform.root.tag);
        if (collision.collider.transform.root.tag == "Player")
        {
            Debug.Log("Player Entered");
            collision.collider.transform.root.GetComponent<PlayerHealth>().inNoGoZone = true;
        }
/*        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.gameObject.root.tag = "Player")
            {
                contact.gameObject.root.GetComponent<PlayerHealth>().inNoGoZone = true;
            }
        }
*/
    }
    void OnTriggerExit(Collision collision)
    {
        if (collision.collider.transform.root.tag == "Player")
        {
            collision.collider.transform.root.GetComponent<PlayerHealth>().inNoGoZone = false;
        }
    }
}
