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
			// Is target in FOV and NPC have a clear sight?
			if (inFOVCondition && !controller.BlockedSight())
			{
				//parker - adding delay - stagger seconds to add levels of reaction
				if (waitTimer > 6)
				{
                    Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                    viewArch.material.SetColor("_BaseColor", red);
                    waitTimer = 0;
					Debug.Log("Found Target Look");
					// Set current target parameters.
					controller.targetInSight = true;
					controller.personalTarget = controller.aimTarget.position;
					return true;
				}else if (waitTimer > 3)
				{
                    Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                    viewArch.material.SetColor("_BaseColor", yellow);

                }
				waitTimer += Time.deltaTime;

			}
			else
			{
                Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
                viewArch.material.SetColor("_BaseColor", green);
                waitTimer = 0;
            }

		}
		// No target on sight.
		return false;
	}
}
