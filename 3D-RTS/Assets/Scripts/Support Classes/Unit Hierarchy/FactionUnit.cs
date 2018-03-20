using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class FactionUnit : DynamicUnit
{
    private bool deathAnimTriggered;
    private bool attackAnimTriggered;
    private bool dealtDamage;

    public FactionUnit(GameObject obj, float health, int[] stats, bool isPlayerControlled , string className) : base(obj, health, isPlayerControlled , className)
    {
        deathAnimTriggered = false;
        attackAnimTriggered = false;
        dealtDamage = false;
        strength = stats[0];
        intelligence = stats[1];
        dexterity = stats[2];
        level = 1;
        experience = 0;
        name = "TN" + ((int) Random.Range(0, 999)).ToString();
    }

    public FactionUnit(string className) : base(className)
    {

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

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        {
            anim.SetBool("TakeDamage", false);
        }

        if (IsAttacking && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !attackAnimTriggered)
        {
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
