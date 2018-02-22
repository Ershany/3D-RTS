using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class DynamicUnit : Unit {

    public bool IsPlayerControlled { get; protected set; }
    protected NavMeshAgent agent;
    protected Animator anim;

    public DynamicUnit(GameObject obj, float health, bool playerControlled) : base(obj, health)
    {
        IsPlayerControlled = playerControlled;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
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

}
