using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour {

    private NavMeshAgent agent;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        agent.destination = new Vector3(350, transform.position.y, 350);
	}
}
