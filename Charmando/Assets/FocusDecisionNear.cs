﻿using UnityEngine;
using EnemyAI;

// The decision to focus on the target.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/FocusNear")]
public class FocusDecisionNear : Decision
{
	[Tooltip("Which sense radius will be used?")]
	public Sense sense;
	[Tooltip("Invalidate current cover when target is spotted?")]
	public bool invalidateCoverSpot;

	private float radius;          // The sense radius that will be used.
    // NPC Sense types.
	public enum Sense
	{
		NEAR,
		PERCEPTION,
		VIEW
	}

	// The decision on enable function, triggered once after a FSM state transition.
	public override void OnEnableDecision(StateController controller)
	{
		// Define sense radius.
		switch (sense)
		{
			case Sense.NEAR:
				radius = controller.nearRadius;
				break;
			case Sense.PERCEPTION:
				radius = controller.perceptionRadius;
				break;
			case Sense.VIEW:
				radius = controller.viewRadius;
				break;
		}
	}
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		// If target is not near: felt a shot and sight to target is clear, can focus.
		// If target is near, always check sense for target.
		return (sense != Sense.NEAR && controller.variables.feelAlert && !controller.BlockedSight()) ||
			Decision.CheckTargetsInRadius(controller, radius, MyHandleTargets);
	}
	// The delegate for results of overlapping targets in focus decision.
	private bool MyHandleTargets(StateController controller, bool hasTargets, Collider[] targetsInHearRadius)
	{
		
        		// Is there any evidence noticed?
		if (hasTargets)
		{
			//Debug.Log("Has Targets Near: " + targetsInHearRadius[0].gameObject.name + " " + targetsInHearRadius[0].gameObject.tag + " " + targetsInHearRadius[0].gameObject.layer);
				//parker adding check for audio source actually playing
				AudioSource sound = targetsInHearRadius[0].gameObject.GetComponentInParent<AudioSource>();
			if (sound.isPlaying && targetsInHearRadius[0].gameObject.tag == "SoundNear")
        	{
                Debug.Log("Near Sound is playing");
			// Invalidate current cover spot (ex.: used to move from position when spotted).
			    if(invalidateCoverSpot)
				controller.CoverSpot = Vector3.positiveInfinity;
			// Set current target parameters.
			    controller.targetInSight = true;
			    controller.personalTarget = controller.aimTarget.position;
			    return true;
			}
		}
		// No moving evidence was noticed.
		return false;
        
        
        
        
        
        
        
        
        // Is there any target, with a clear sight to it?
		/*if (hasTargets && !controller.BlockedSight())
		{
			// Invalidate current cover spot (ex.: used to move from position when spotted).
			if(invalidateCoverSpot)
				controller.CoverSpot = Vector3.positiveInfinity;
			// Set current target parameters.
			controller.targetInSight = true;
			controller.personalTarget = controller.aimTarget.position;
			return true;
		}
		// No target on sight.
		return false;*/
	}
}
