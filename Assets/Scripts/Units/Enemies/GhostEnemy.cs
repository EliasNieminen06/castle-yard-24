using UnityEngine;
using System.Collections;

public class GhostEnemy : Enemy
{
    private void FixedUpdate()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * Stats.Speed;

        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, Vector3.up));
    }
}
