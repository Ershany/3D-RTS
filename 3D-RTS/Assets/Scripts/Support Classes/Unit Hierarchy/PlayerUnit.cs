using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PlayerUnit : DynamicUnit {

    public PlayerController playerController;

    private bool deathAnimTriggered;
    private bool attackAnimTriggered;

	public PlayerUnit(GameObject obj, float health, int[] stats) : base(obj, health, true)
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        deathAnimTriggered = false;
        attackAnimTriggered = false;
        strength = stats[0];
        intelligence = stats[1];
        dexterity = stats[2];
    }

    public void Update()
    {
        if (IsDead)
        {
            StopMovement();
            collider.enabled = false;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        if (IsAttacking)
        {
            IsInBattle = true;
            Debug.Log("Destination: " + agent.destination.ToString());
            Debug.Log("Position: " + GetTransform().position.ToString());

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {

                IsAttacking = false;
                anim.SetBool("Attacking", false);
            }
        }
        AnimateUnit();
    }

    private void AnimateUnit()
    {   
        anim.SetBool("Moving", agent.velocity.sqrMagnitude > 0 ? true : false);
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);

        anim.SetBool("Combat", true);

        Debug.Log("Distance Between " + Vector3.Distance(agent.destination, GetTransform().position).ToString());
        Debug.Log("Local Scale" + GetTransform().localScale.x.ToString());

        if (IsAttacking && Vector3.Distance(agent.destination, GetTransform().position) < GetTransform().localScale.x && !attackAnimTriggered)
        {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            anim.SetBool("Attacking", true);
            
            attackAnimTriggered = true;
        }

        if (IsDead && !deathAnimTriggered)
        {
            anim.SetTrigger("Die");
            deathAnimTriggered = true;
        }
    }
}
