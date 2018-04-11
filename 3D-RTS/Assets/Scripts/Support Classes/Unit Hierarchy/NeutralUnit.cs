using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class NeutralUnit : DynamicUnit
{

    public float unitDamage;

    private bool deathAnimTriggered;
    private bool attackAnimTriggered;
    private bool dealtDamage;

    public NeutralUnit(GameObject obj, float health , float damage , string name) : base(obj, false, "Neutral Unit")
    {
        IsInBattle = false;
        strength = (int) damage;
        dexterity = 5;
        intelligence = 5;
        maxMana = 10;
        currentMana = maxMana;
        currentHealth = maxHealth;
        this.name = name;
        anim = obj.GetComponent<Animator>();
    }



    public void Update()
    {
        if (IsDead)
        {
            agent.isStopped = true;
            gameObject.GetComponent<SphereCollider>().enabled = false;
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
            agent.SetDestination(gameObject.transform.position);
            attackAnimTriggered = true;
        }

        if (IsDead && !deathAnimTriggered)
        {
            anim.SetTrigger("Die");
            deathAnimTriggered = true;
        }
    }
    
}
