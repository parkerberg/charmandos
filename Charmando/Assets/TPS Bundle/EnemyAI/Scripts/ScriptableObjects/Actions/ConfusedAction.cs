using UnityEngine;
using EnemyAI;

// The search for point of interest action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Confused")]
public class ConfusedAction : Action
{
	private static readonly int Crouch = Animator.StringToHash("Crouch");
    private Vector3 point;
    private Vector3 point2;

    private Vector3 currentPoint;

    private float waitTime;
    private bool searchedTwice = false;
    public LayerMask mask = new LayerMask();
    private Collider[] targetsInRadius;

    public float randomPointRadius = 10f;
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
    waitTime += Time.deltaTime;
    if(waitTime >= 10f){
		if (Equals(controller.personalTarget, Vector3.positiveInfinity))
			controller.nav.destination = controller.transform.position;
		else
		{
			// Set navigation parameters.
			controller.nav.speed = controller.generalStats.chaseSpeed;
            
            controller.nav.destination = currentPoint;
           // controller.nav.destination = controller.personalTarget;
		}

        if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending && controller.confusedCount == 1)
		{
			controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
                Debug.Log("First Points");
				//execute next waypoint
                controller.variables.patrolTimer = 0f;
			    controller.completedSearch = true;

			}
		}else if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending && controller.confusedCount > 1 && searchedTwice == false){
			controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
				//execute next waypoint
                Debug.Log("Second Points");
				controller.variables.patrolTimer = 0f;
                currentPoint = point2;
                searchedTwice = true;
			}
        }else if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending && searchedTwice == true){
			controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
				//execute next waypoint
                Debug.Log("Second Point done");
                controller.variables.patrolTimer = 0f;
				controller.completedSearch = true;
			}
        }
    }
     

	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{

        controller.confusedCount += 1;
        //point = (Random.insideUnitSphere * randomPointRadius) + controller.transform.position;
        
		controller.confusedSymbol.SetActive(true);
		controller.alertSymbol.SetActive(false);

		controller.focusSight = false;
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool(Crouch, false);
		controller.CoverSpot = Vector3.positiveInfinity;

        //check the distance to the waypoint
        //pick the closest
        targetsInRadius =
			Physics.OverlapSphere(controller.transform.position, controller.viewRadius, mask);
            

        if(targetsInRadius.Length > 0){

                point = targetsInRadius[0].transform.position;
               
                
         }
        if(targetsInRadius.Length > 1){
             if (controller.confusedCount == 2){
                point2 = targetsInRadius[1].transform.position;
            }
        }
        currentPoint = point;
        
        searchedTwice = false;
        
        

        Debug.Log("point" + point);


	}
}
