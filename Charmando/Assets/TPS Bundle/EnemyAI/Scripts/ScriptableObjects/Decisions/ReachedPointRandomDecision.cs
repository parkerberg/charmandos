using UnityEngine;
using EnemyAI;

// The decision of whether or not the spot was reached.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Reached Point Random")]
public class ReachedPointRandomDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
    public float maxTimeToWait = 10f;
    private float timeToWait;
    private float startTime;
	public override bool Decide(StateController controller)
	{
		if (controller.completedSearch)
		{
			Debug.Log("FinishedWaypoint.");
                if((Time.time - startTime) >= timeToWait){
                Debug.Log("FinishedWaypoint. Post Waited");
                controller.completedSearch = false;
                return true;
                }
            
            
		}

			return false;
		
	}

    public override void OnEnableDecision(StateController controller)
	{
		// Calculate time to wait on current round.
		timeToWait = Random.Range(3, maxTimeToWait);
		// Set start waiting time.
		startTime = Time.time;
	}
}
