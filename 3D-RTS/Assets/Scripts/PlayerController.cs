using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject guildHallPrefab;
    public Group SelectedGroup { get; private set; }

    private List<Group> groups;
    private Building buildingSelected;

    private int terrainMask;

    void Awake()
    {
        groups = new List<Group>();
        SelectedGroup = null;
        terrainMask = LayerMask.GetMask("TerrainLayer");
    }
	
	void Update ()
    {
        // Shoot a raycast at the mouse position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, float.MaxValue, terrainMask);

        // TEMP Building Selection Code
        if (Input.GetKeyDown("1") && buildingSelected == null)
        {
            buildingSelected = Instantiate(guildHallPrefab, Vector3.zero, Quaternion.Euler(-90.0f, 0.0f, 0.0f)).GetComponent<GuildHallController>().building;
            buildingSelected.MoveBuilding(hit.point);
        }
        // TEMP Unit Death Testing
        if (Input.GetKeyDown("k"))
        {
            SelectedGroup.GroupTakeDamage(5);
        }

        // Move building with cursor if a building is currently selected (keep it on the terrain)
        if (buildingSelected != null)
        {
            buildingSelected.MoveBuilding(hit.point);
        }

        // Check if the player issued a command by clicking and act accordingly
        if (Input.GetMouseButtonDown(0))
        {
            if (buildingSelected != null) // Check if the player is placing a building
            {
                if (buildingSelected.PlaceBuildingOnTerrain())
                {
                    buildingSelected = null;
                    Debug.Log("Building Placed");
                }
            }
            else if (SelectedGroup != null) // Check if the player is commanding a group of units
            {
                Transform objectHit = hit.transform;
                SelectedGroup.SetGroupDestination(hit.point);
                Debug.Log("Group is moving to " + hit.point.ToString());
            }
        }

        // Check if the player is cancelling the command to place a building
        if (Input.GetMouseButtonDown(1) && buildingSelected != null)
        {
            Destroy(buildingSelected.GameObject);
            buildingSelected = null;
            Debug.Log("Cancelling Building Placement");
        }
	}
    
    public void AddGroup(Group group)
    {
        groups.Add(group);
        SelectedGroup = group;
    }
}