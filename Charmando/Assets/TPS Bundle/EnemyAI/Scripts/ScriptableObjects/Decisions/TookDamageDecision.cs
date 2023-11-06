using UnityEngine;
using EnemyAI;

// The decision to check if sight to target is clear.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Took Damage")]
public class TookDamageDecision : Decision
{
	[Header("Extra Decisions")]
	[Tooltip("The NPC took damage sense.")]
	public FocusDecision targetNear;

	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		return targetNear.Decide(controller) || HaveClearShot(controller);
	}
	// Cast sphere for near obstacles, and line to personal target (not the aim target) for clean shot.
	private bool HaveClearShot(StateController controller)
	{
		return false;

        //grab attribute script, or some other check for damage.
	}
}
