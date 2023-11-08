using UnityEngine;
using EnemyAI;

// The decision to hear an evidence. Sense of hearing.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Hear")]
public class HearDecision : Decision
{
	private Vector3 lastPos, currentPos;   // Last and current evidence positions.

	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		// Handle external alert received.
		if(controller.variables.hearAlert)
		{
			controller.variables.hearAlert = false;
			return true;
		}
		// Check if something was heard by the NPC.
		else
			return Decision.CheckTargetsInRadius(controller, controller.perceptionRadius, MyHandleTargets);
	}
	// The decision on enable function, triggered once after a FSM state transition.
	public override void OnEnableDecision(StateController controller)
	{
		lastPos = currentPos = Vector3.positiveInfinity;
	}
	// The delegate for results of overlapping targets in hear decision.
	private bool MyHandleTargets(StateController controller, bool hasTargets, Collider[] targetsInHearRadius)
	{
		// Is there any evidence noticed?
		if (hasTargets)
		{
			if(targetsInHearRadius[0].gameObject.tag == "Sound")
			{
                Debug.Log("Tagged as sound" + targetsInHearRadius[0].gameObject.tag);
                //loop through targets and check for stuff.
                // Grab current evidence position.
                currentPos = targetsInHearRadius[0].transform.position;
                // Evidence is already on track, check if it has moved.
                if (!Equals(lastPos, Vector3.positiveInfinity))
                {
                    // The hear sense is only triggered if the evidence is in movement.
                    if (!Equals(lastPos, currentPos))
                    {
                        controller.personalTarget = currentPos;
                        return true;
                    }
                }
                // Set evidence position for next game loop.
                lastPos = currentPos;
            }
			//Debug.Log("Has Targets Hear: " + targetsInHearRadius[0].gameObject.name + " " + targetsInHearRadius[0].gameObject.tag + " " + targetsInHearRadius[0].gameObject.layer);
				//parker adding check for audio source actually playing
				AudioSource sound = targetsInHearRadius[0].gameObject.GetComponentInParent<AudioSource>();
            Debug.Log("NotPlaying" + targetsInHearRadius[0].gameObject.layer + " name " + targetsInHearRadius[0].gameObject.name);
            if (sound.isPlaying)
        	{
				Debug.Log("Sound is playing hear" + targetsInHearRadius[0].gameObject.layer);
			//loop through targets and check for stuff.
			// Grab current evidence position.
			currentPos = targetsInHearRadius[0].transform.position;
			// Evidence is already on track, check if it has moved.
			if (!Equals(lastPos, Vector3.positiveInfinity))
			{
				// The hear sense is only triggered if the evidence is in movement.
				if(!Equals(lastPos, currentPos))
				{
					controller.personalTarget = currentPos;
					return true;
				}
			}
			// Set evidence position for next game loop.
				lastPos = currentPos;
			}
		}
		// No moving evidence was noticed.
		return false;
	}
}
