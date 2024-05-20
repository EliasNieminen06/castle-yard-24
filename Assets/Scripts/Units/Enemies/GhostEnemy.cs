using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GhostEnemy : Enemy
{
    [SerializeField] private NavMeshAgent agent;

    public override void Init()
    {
        base.Init();

        if (agent == null)
        {
            Debug.LogWarning("Navmesh On " + this.gameObject + " is null");
            agent = GetComponent<NavMeshAgent>();
        }

        agent.speed = Stats.Speed;
    }

    private void FixedUpdate()
    {
        agent.SetDestination(player.position);

        //Vector3 direction = (player.position - transform.position).normalized;
        //rb.velocity = direction * Stats.Speed;
        //
        //transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, Vector3.up));
    }
}
