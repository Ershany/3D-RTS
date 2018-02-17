using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class DynamicUnit : Unit {

    public bool IsPlayerControlled { get; protected set; }
    protected NavMeshAgent agent;

    public DynamicUnit(float health, bool playerControlled) : base(health)
    {
        IsPlayerControlled = playerControlled;
        agent = GetComponent<NavMeshAgent>();
    }
	
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

}
