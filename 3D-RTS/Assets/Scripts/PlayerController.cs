using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerUnit selectedUnit; // TEMP should be selectedGroup and should be private

    void Awake()
    {
        selectedUnit = null;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && selectedUnit != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                selectedUnit.SetDestination(hit.point);
                Debug.Log("Unit is moving to " + hit.point.ToString());
            }
        }
	}
}