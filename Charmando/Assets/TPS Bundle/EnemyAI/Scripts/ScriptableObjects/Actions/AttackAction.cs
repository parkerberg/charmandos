
namespace Opsive.UltimateCharacterController.Demo.Objects
{
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
using UnityEngine;
using EnemyAI;

// The NPC attack action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Attack")]

public class AttackAction : Action
{
	private readonly float startShootDelay = 0.2f; // Delay before start shooting.
	private readonly float aimAngleGap = 30f;      // Minimum angle gap between current and desired aim orientations.
	private static readonly int Shooting = Animator.StringToHash("Shooting");
	private static readonly int Crouch = Animator.StringToHash("Crouch");
      [SerializeField] protected float m_VelocityMagnitude = 10;
	  [SerializeField] protected float m_ShotDistance = 650f;

	  
	  [SerializeField] protected float m_FocusDistance = 725f;
        [SerializeField] protected ImpactDamageData m_ImpactDamageData = new ImpactDamageData()
        {
            LayerMask = ~(1 << LayerManager.IgnoreRaycast | 1 << LayerManager.TransparentFX | 1 << LayerManager.UI | 1 << LayerManager.Overlay),
            DamageAmount = 10,
            ImpactForce = 0.05f,
            ImpactForceFrames = 1,
        };
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		// Always focus on sight position.
		controller.focusSight = true;

		if (CanShoot(controller))
		{
			Shoot(controller);
		}
		// Accumulate blind engage timer.
		controller.variables.blindEngageTimer += Time.deltaTime;
	}
	// Can the NPC shoot?
	private bool CanShoot(StateController controller)
	{
		// NPC is aiming and almost aligned with desired position?
		//parker - modified this to check how far away the character is and to lose focus if out of the radius of their view. Checks if they are in focus range and then shot range 
		/*if((controller.personalTarget - controller.enemyAnimation.gunMuzzle.position).sqrMagnitude >= m_FocusDistance)
		{
					controller.focusSight = false;
		controller.variables.feelAlert = false;
		controller.variables.hearAlert = false;
		controller.Strafing = false;
		controller.nav.destination = controller.personalTarget;
		controller.nav.speed = 0f;
			return false;
		}else*/
		 if (controller.Aiming && 
			(controller.enemyAnimation.currentAimAngleGap < aimAngleGap ||
			// Or if the target is too close, shot anyway
			(controller.personalTarget - controller.enemyAnimation.gunMuzzle.position).sqrMagnitude <= 0.25f))
		{
			// All conditions match, check start delay.
			if (controller.variables.startShootTimer >= startShootDelay)
			{
				return true;
			}
			else
			{
				controller.variables.startShootTimer += Time.deltaTime;
			}
		}
		return false;
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.variables.shotsInRound = Random.Range(controller.maximumBurst / 2, controller.maximumBurst);
		controller.variables.currentShots = 0;
		controller.variables.startShootTimer = 0f;
		controller.enemyAnimation.anim.ResetTrigger(Shooting);
		controller.enemyAnimation.anim.SetBool(Crouch, false);
		controller.variables.waitInCoverTimer = 0;
		controller.enemyAnimation.ActivatePendingAim();
	}
	// Perform the shoot action.
	private void Shoot(StateController controller)
	{
		// Check interval between shots.
		if (Time.timeScale > 0 && controller.variables.shotTimer == 0f)
		{
			controller.enemyAnimation.anim.SetTrigger(Shooting);
			CastShot(controller);
		}
		// Update shot related variables and habilitate next shot.
		else if(controller.variables.shotTimer >= (0.1f + 2 * Time.deltaTime))
		{
			controller.bullets = Mathf.Max(--controller.bullets, 0);
			controller.variables.currentShots++;
			controller.variables.shotTimer = 0f;
			return;
		}
		controller.variables.shotTimer += controller.classStats.shotRateFactor * Time.deltaTime;
	}
	// Cast the shot.
	private void CastShot(StateController controller)
	{
		// Get shot imprecision vector.
		Vector3 imprecision = Random.Range(-controller.classStats.shotErrorRate, controller.classStats.shotErrorRate)
			* controller.transform.right;

		imprecision += Random.Range(-controller.classStats.shotErrorRate, controller.classStats.shotErrorRate)
			* controller.transform.up;
		// Get shot desired direction.
		Vector3 shotDirection = controller.personalTarget - controller.enemyAnimation.gunMuzzle.position;
		// Cast shot.
		Ray ray = new Ray(controller.enemyAnimation.gunMuzzle.position, shotDirection.normalized + imprecision);

		//GameObject g = ObjectPoolBase.Instantiate(controller.classStats.projectile, controller.enemyAnimation.gunMuzzle.position,
		//Quaternion.LookRotation(controller.personalTarget)
		//) as GameObject;	

		//Projectile projectile = g.GetComponent<Projectile>();
		//projectile.Initialize(0, -controller.enemyAnimation.gunMuzzle.right * m_VelocityMagnitude, Vector3.zero, controller.gameObject, m_ImpactDamageData);

		var projectile = ObjectPoolBase.Instantiate(controller.classStats.projectile, controller.enemyAnimation.gunMuzzle.position, Quaternion.LookRotation(controller.personalTarget)).GetCachedComponent<Projectile>();
    	projectile.Initialize(0, -controller.enemyAnimation.gunMuzzle.right * m_VelocityMagnitude, Vector3.zero, controller.gameObject, m_ImpactDamageData);
		
		//projectile = Instantiate(controller.classStats.projectile, controller.enemyAnimation.gunMuzzle.position, Quaternion.identity);
		//projectile.Initialize(0, m_FireLocation.forward * m_VelocityMagnitude, Vector3.zero, m_GameObject, m_ImpactDamageData);
		if (Physics.Raycast(ray, out RaycastHit hit, controller.viewRadius, controller.generalStats.shotMask.value))
		{
			// Hit something organic? Consider all layers in target mask as organic.
			bool isOrganic = ((1 << hit.transform.root.gameObject.layer) & controller.generalStats.targetMask) != 0;
			DoShot(controller, ray.direction, hit.point, hit.normal, isOrganic, hit.transform);
		}
		else
		{
			// Hit nothing (miss shot), shot at desired direction with imprecision.
			DoShot(controller, ray.direction, ray.origin + (ray.direction * 500f));
		}
	}


	// Draw shot and extra assets.
	
	private void DoShot(StateController controller, Vector3 direction, Vector3 hitPoint,
		Vector3 hitNormal = default, bool organic = false, Transform target = null)
	{
		// Draw muzzle flash.
		GameObject muzzleFlash = Instantiate(controller.classStats.muzzleFlash, controller.enemyAnimation.gunMuzzle);
		muzzleFlash.transform.localPosition = Vector3.zero;
		muzzleFlash.transform.localEulerAngles = Vector3.back * 90f;
		controller.StartCoroutine(this.DestroyFlash(muzzleFlash));

		// Draw shot tracer and smoke.
		GameObject shotTracer = Instantiate(controller.classStats.shot, controller.enemyAnimation.gunMuzzle);
		// Padding to start shot tracer
		Vector3 origin = controller.enemyAnimation.gunMuzzle.position - controller.enemyAnimation.gunMuzzle.right * 0.5f;
		shotTracer.transform.position = origin;
		shotTracer.transform.rotation = Quaternion.LookRotation(direction);

		// Draw bullet hole and sparks, if target is not organic.
		if(target && !organic)
		{
			GameObject bulletHole = Instantiate(controller.classStats.bulletHole);
			bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
			bulletHole.transform.position = hitPoint + 0.01f * hitNormal;
			GameObject instantSparks = Instantiate(controller.classStats.sparks);
			instantSparks.transform.position = hitPoint;
		}
		// The object hit is organic, call take damage function.
		else if(target && organic)
		{
			//Instantiate(controller.classStats.projectile, target.position, Quaternion.identity);	
			HealthManager targetHealth = target.GetComponent<HealthManager>();
			if(targetHealth)
			{
				//This is where you would make the change for dealing damage to the UCC character.
				targetHealth.TakeDamage(hitPoint, direction, controller.classStats.bulletDamage, target.GetComponent<Collider>(), controller.gameObject);
			}
		}
		// Play shot audio clip at shot position.
		AudioSource.PlayClipAtPoint(controller.classStats.shotSound, controller.enemyAnimation.gunMuzzle.position, 2f);
	}
	// Function to destroy the muzzle flash.
	public IEnumerator DestroyFlash(GameObject flash)
	{
		yield return new WaitForSeconds(0.1f);
		Destroy(flash);
	}
}
}
