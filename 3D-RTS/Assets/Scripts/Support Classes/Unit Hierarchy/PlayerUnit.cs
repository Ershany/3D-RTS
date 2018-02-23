using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PlayerUnit : DynamicUnit {

    public PlayerController playerController;

    private bool deathAnimTriggered;

	public PlayerUnit(GameObject obj, float health) : base(obj, health, true)
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
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
