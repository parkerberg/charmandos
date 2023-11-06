using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDeathEffect : MonoBehaviour
{


    public Animator anim;

    public Rigidbody body;
    public GameObject weapon;
    public GameObject enemyGameobject;

	public Canvas ui;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Kill()
		{
			// Destroy all other MonoBehaviour scripts attached to the NPC.
			foreach (MonoBehaviour mb in enemyGameobject.GetComponents<MonoBehaviour>())
			{
				if (enemyGameobject != mb)
					Destroy(mb);
			}
			Destroy(enemyGameobject.GetComponent<UnityEngine.AI.NavMeshAgent>());
			RemoveAllForces();
			anim.enabled = false;
            Destroy(weapon.gameObject);
			ui.enabled = false;
			//Destroy(weapon.gameObject);
			//Destroy(hud.gameObject);
			//dead = true;
			//hips.tag = "Dead";
		}

    private void RemoveAllForces()
		{
			foreach (Rigidbody member in enemyGameobject.GetComponentsInChildren<Rigidbody>())
			{
				member.isKinematic = false;
				member.velocity = Vector3.zero;
			}
		}
}
