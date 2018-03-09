using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class DemonUnit : DynamicUnit
{
    public bool deathAnimTriggered;

    public DemonUnit(GameObject obj, float health) : base(obj, health, false)
    {
        deathAnimTriggered = false;
    }

    public void Update()
    {
        if (IsDead)
        {
            StopMovement();
            collider.enabled = false;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        AnimateUnit();
    }

    private void AnimateUnit()
    {
        anim.SetBool("Moving", agent.velocity.sqrMagnitude > 0 ? true : false);
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);

        if (IsDead && !deathAnimTriggered)
        {
            anim.SetTrigger("Die");
            deathAnimTriggered = true;
        }
    }
}
