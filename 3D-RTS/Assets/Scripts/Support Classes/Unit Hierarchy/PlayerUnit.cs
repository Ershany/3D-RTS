using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PlayerUnit : DynamicUnit
{

    public PlayerController playerController;

    private bool deathAnimTriggered;
    private bool attackAnimTriggered;
    private bool dealtDamage;

    public PlayerUnit(GameObject obj, float health, int[] stats) : base(obj, health, true)
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        deathAnimTriggered = false;
        attackAnimTriggered = false;
        dealtDamage = false;
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
            //Debug.Log("Destination: " + agent.destination.ToString());
            //Debug.Log("Position: " + GetTransform().position.ToString());

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackAnimation"))
            {

                if (anim.GetCurrentAnimatorStateInfo(0).length >= 0.45f)
                {
                    if (dealtDamage == false)
                    {
                        targetUnit.TakeDamage(5.0f);
                        dealtDamage = true;
                    }
                }

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {

                    dealtDamage = false;
                    attackAnimTriggered = false;
                    IsAttacking = false;
                    anim.SetBool("Attacking", false);
                }
            }
        }
        AnimateUnit();
    }

    private void AnimateUnit()
    {
        anim.SetBool("Moving", agent.velocity.sqrMagnitude > 0 ? true : false);
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);

        anim.SetBool("Combat", IsInBattle ? true : false);

        //Debug.Log("Distance Between " + Vector3.Distance(agent.destination, GetTransform().position).ToString());
        //Debug.Log("Local Scale" + GetTransform().localScale.x.ToString());

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamageAnimation"))
        {
            anim.SetBool("TakeDamage", false);
        }

        if (IsAttacking && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !attackAnimTriggered)
        {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            anim.SetBool("Attacking", true);
            Halt();
            attackAnimTriggered = true;
        }

        if (IsDead && !deathAnimTriggered)
        {
            anim.SetTrigger("Die");
            deathAnimTriggered = true;
        }
    }
}
