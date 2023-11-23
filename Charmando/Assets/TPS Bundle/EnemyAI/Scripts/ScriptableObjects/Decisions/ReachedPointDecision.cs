using UnityEngine;
using EnemyAI;

// The decision of whether or not the spot was reached.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Reached Point")]
public class ReachedPointDecision : Decision
{
    public float maxTimeToWait = 1f;
    private float timeToWait;
    private float startTime;
    // The decide function, called on Update() (State controller - current state - transition - decision).
    public override bool Decide(StateController controller)
	{
		if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending)
		{
            if ((Time.time - startTime) >= timeToWait)
            {
                Debug.Log("FinishedWaypoint. Reached Post Waited");
                return true;
            }
            return false;
		}
		else
		{
			return false;
		}
	}
    public override void OnEnableDecision(StateController controller)
    {
        // Calculate time to wait on current round.
        //timeToWait = Random.Range(3, maxTimeToWait);
        timeToWait = maxTimeToWait;
        // Set start waiting time.
        startTime = Time.time;
    }
}
