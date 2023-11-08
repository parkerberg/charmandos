namespace Opsive.UltimateCharacterController.Demo.Objects
{
	using UnityEngine;
	using EnemyAI;
	using System.Collections;
	using Opsive.Shared.Game;
	using Opsive.UltimateCharacterController.Character;
	using Opsive.UltimateCharacterController.Game;
	using Opsive.UltimateCharacterController.Items.Actions.Impact;
#if ULTIMATE_CHARACTER_CONTROLLER_VERSION_2_MULTIPLAYER
    using Opsive.UltimateCharacterController.Networking;
    using Opsive.UltimateCharacterController.Networking.Game;
#endif
	using Opsive.UltimateCharacterController.Objects;
	using Opsive.UltimateCharacterController.Traits;
	using Opsive.UltimateCharacterController.Utility;

	// The decision of whether or not the spot was reached.
	[CreateAssetMenu(menuName = "Enemy AI/Decisions/Taking Damage")]
	public class TakingDamageDecision : Decision
	{
		private float lastHealth;
		private float currentHealth;

		// The decide function, called on Update() (State controller - current state - transition - decision).
		public override bool Decide(StateController controller)
		{
			currentHealth = controller.gameObject.GetComponent<AttributeManager>().Attributes[0].Value;

			if (currentHealth < lastHealth)
			{
				lastHealth = currentHealth;
				return true;
			}
			else
			{
				lastHealth = currentHealth;
				return false;
			}
		}
	}
}
