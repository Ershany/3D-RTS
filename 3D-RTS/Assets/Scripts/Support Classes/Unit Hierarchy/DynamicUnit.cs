using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class DynamicUnit : Unit {

    public bool IsPlayerControlled { get; protected set; }
    protected NavMeshAgent agent;
    protected Animator anim;

    protected int dexterity;
    protected int strength;
    protected int intelligence;
    public float attackTime { get; set; }
    public bool IsAttacking { get; protected set; }
    public bool IsInBattle { get;  set; }
    private DynamicUnit targetUnit;
    private List<DynamicUnit> opponents;

    public DynamicUnit(GameObject obj, float health, bool playerControlled) : base(obj, health)
    {
        IsPlayerControlled = playerControlled;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        IsAttacking = false;
        IsInBattle = false;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }
	
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }

    public void Halt()
    {
        agent.SetDestination(GetTransform().position);
    }


    public void BeginAttack(List<DynamicUnit> targets)
    {
        opponents = targets;
    }

    public void AttackAnyEnemy()
    {
        IsAttacking = true;
        for (int i = 0; i < opponents.Count; i++)
        {
            if(!opponents[i].IsDead)
            {
                Debug.Log("4");
                targetUnit = opponents[i];
                SetDestination(opponents[i].GetTransform().position);
                break;
            }
        }
    }

    public int GetDexterity()
    {
        return dexterity;
    }
    public int GetStrength()
    {
        return strength;
    }
    public int GetIntelligence()
    {
        return intelligence;
    }

}
