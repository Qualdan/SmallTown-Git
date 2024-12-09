using UnityEngine;
using UnityEngine.AI;

namespace SmallTown {
public static class AgentExtensions {


	// ************************************************************************************************************ EXTENSIONS

	public static bool CanReach(this NavMeshAgent navMeshAgent, Vector3 target) {
		NavMeshPath path = new NavMeshPath();
		if(navMeshAgent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete) {
			return true;
		}
		return false;
	}

	public static bool HasFinished(this NavMeshAgent navMeshAgent, float distance = 0.0f) {
		if (!navMeshAgent.pathPending) {
			if(!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
				return true;
			}
			bool checkDistance = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
			if (distance > 0.0f) {
				checkDistance = navMeshAgent.remainingDistance <= distance;
			}
			if (checkDistance) {
				return true;
			}
		}
		return false;
	}
}
}