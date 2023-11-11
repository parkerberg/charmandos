using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

// The NPC patrol action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Patrol")]
public class PatrolAction : Action
{
	    private Color green = new Color(0, 1, 0, 0.45f);
		private int count = 0;
	private static readonly int Crouch = Animator.StringToHash("Crouch");

	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		Patrol(controller);
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool(Crouch, false);
		controller.personalTarget = Vector3.positiveInfinity;
		controller.CoverSpot = Vector3.positiveInfinity;
		controller.focusSight = false;
		controller.confusedSymbol.SetActive(false);
		controller.alertSymbol.SetActive(false);
		Renderer viewArch = controller.viewArc.GetComponent<Renderer>();
        viewArch.material.SetColor("_BaseColor", green);
	}
	// NPC patrolling function.
	private void Patrol(StateController controller)
	{

		if (controller.patrolWayPoints.Count == 0){
			return;
		}

		
		// No patrol waypoints, stand idle.
		if (controller.patrolWayPoints.Count == 2 && controller.stationaryPatrol){
			

		
		if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending && count == 1)
		{
						controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
			controller.transform.LookAt(controller.patrolWayPoints[1].position);
			return;
			}

			
		}
		controller.nav.destination = controller.patrolWayPoints[0].position;
		count = 1;
			if(controller.searchWayPoints.Count > 0){
				Debug.Log("Waypoints Found");
			//controller.enemyAnimation.LookAtObject(controller.searchWayPoints[0].position);
			}
			return;
		}
			
		// Set navigation parameters.
		controller.focusSight = false;
		controller.nav.speed = controller.generalStats.patrolSpeed;
		// Reached waypoint, wait for a moment before keep patrolling.
		if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending)
		{
			controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
				controller.waypointIndex = (controller.waypointIndex + 1) % controller.patrolWayPoints.Count;
				controller.variables.patrolTimer = 0f;
			}
		}
		// Set next patrol waypoint.
		try
		{
			controller.nav.destination = controller.patrolWayPoints[controller.waypointIndex].position;
		}
		catch (UnassignedReferenceException)
		{
			// Suggest patrol waypoints for NPC, if none.
			Debug.LogWarning("No waypoints assigned for " + controller.transform.name+", enemy will remain idle");
			// No waypoints, create single position to stand still.
			controller.patrolWayPoints = new List<Transform>
			{
				controller.transform
			};
			controller.nav.destination = controller.transform.position;
		}
	}
}
