using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private List<Group> groups;
    private Group selectedGroup;

    void Awake()
    {
        groups = new List<Group>();
        selectedGroup = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetMouseButtonDown(0) && selectedGroup != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                selectedGroup.SetGroupDestination(hit.point);
                Debug.Log("Group is moving to " + hit.point.ToString());
            }
        }
	}
    
    public void AddGroup(Group group)
    {
        groups.Add(group);
        selectedGroup = group;
    }
}