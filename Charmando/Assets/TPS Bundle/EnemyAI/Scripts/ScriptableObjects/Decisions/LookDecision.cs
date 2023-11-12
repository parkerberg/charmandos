using UnityEngine;
using EnemyAI;

// The decision to see the target. Sense of sight.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Look")]
public class LookDecision : Decision
{
    private float waitTimer = 0;
    private Color green = new Color(0, 1, 0, 0.45f);
    private Color yellow = new Color(1, 0.92f, 0.016f, 0.45f);
    private Color red = new Color(1, 0, 0, 0.45f);
	private bool thisSetGlobalArc = false;
	private AiHub hub;
    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
	{
		// Reset sight status on loop before checking.
		controller.targetInSight = false;
		// Check sight.
		return Decision.CheckTargetsInRadius(controller, controller.viewRadius, MyHandleTargets);
	}

	// The delegate for results of overlapping targets in look decision.
	private bool MyHandleTargets(StateController controller, bool hasTargets, Collider[] targetsInViewRadius)
	{
		//Debug.Log(waitTimer);
		// Is there any sight on view radius?
		if(hasTargets)
		{
//			Debug.Log("Has Targets View: " + targetsInViewRadius[0].gameObject.name + " " + targetsInViewRadius[0].gameObject.tag + " " + targetsInViewRadius[0].gameObject.layer);
			Vector3 target = targetsInViewRadius[0].transform.position;
			// Check if target is in field of view.controller.enemyAnimation.head
			//Enemies eyesight
			Debug.DrawRay(controller.enemyAnimation.head.position, -controller.enemyAnimation.head.forward * 1000, Color.yellow);
			//parker adjusted this to work off the head
			Vector3 dirToTarget = target - controller.enemyAnimation.head.position;
			bool inFOVCondition = (Vector3.Angle(dirToTarget, -controller.enemyAnimation.head.forward) < controller.viewAngle / 2);
			//Debug.Log("Distance: " + Vector3.Distance(target, controller.enemyAnimation.head.position));
			bool tooFar = Vector3.Distance(target, controller.enemyAnimation.head.position) >= 28f;
			// Is target in FOV and NPC have a clear sight?
			if (inFOVCondition && !controller.BlockedSight() && !tooFar)
			{
				//Debug.Log("Global Bool" + AiHub.globalArcEnabled);
				if (AiHub.lastEnemyViewing != controller.gameObject && !AiHub.globalArcEnabled)
				{
					AiHub.SetGlobalArc(controller.gameObject, controller.viewArc);
				}
				if(AiHub.globalArcEnabled){
					waitTimer = 0;
					controller.targetInSight = true;
					controller.personalTarget = controller.aimTarget.position;
					return true;
				}
		
				
/*				if(thisSetGlobalArc == false)
				{
					thisSetGlobalArc = true;
					//controller.aiHub.GetComponent<AiHub>().globalArcEnabled = true;
					AiHub.globalArcEnabled = true;
                controller.GetComponent<FieldOfView>().enabled = true;
				controller.viewArc.SetActive(true);
				}*/
                //parker - adding delay - stagger seconds to add levels of reaction
                if (waitTimer > 2 || controller.currentState.name != "PatrolState")
				{
					AiHub.globalArcEnabled = true;
					Vector3[] alertPack = { controller.transform.position, controller.aimTarget.position };
                    GameObject.FindGameObjectWithTag("GameController").SendMessage("RootAlertMedium", alertPack, SendMessageOptions.DontRequireReceiver);
                    //Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                    //viewArch.material.SetColor("_BaseColor", red);
                    waitTimer = 0;
					Debug.Log("Found Target Look");
					// Set current target parameters.
					controller.targetInSight = true;
					controller.personalTarget = controller.aimTarget.position;
					return true;
				}else if (waitTimer > 0.5f)
				{
                    Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                    viewArch.material.SetColor("_BaseColor", yellow);

                }
				waitTimer += Time.deltaTime;

			}else if(controller.currentState.name == "PatrolState" && controller.viewArc.activeSelf)
		{
				Debug.Log("Turn Green");
                Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                viewArch.material.SetColor("_BaseColor", green);
                waitTimer = 0;
				//AiHub.SetGlobalArc(controller.gameObject, controller.viewArc);
/*				if (thisSetGlobalArc)
				{
                    //controller.aiHub.GetComponent<AiHub>().globalArcEnabled = false;
                    AiHub.globalArcEnabled = false;
                    thisSetGlobalArc = false;
                }*/
         }


		}
		
		
		/*else if(!controller.viewArcActive){ // need and else if the player has activated it manually
				controller.GetComponent<FieldOfView>().enabled = false;
				controller.viewArc.SetActive(false);
		}*/
		
		// No target on sight.
		return false;
	}
}
