using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //prefabs
    public GameObject guildHallPrefab;
    public GameObject arenaPrefab;

    //selected group of units
    public Group selectedGroup { get; private set; }

    //players groups 
    public List<Group> groups { get; private set; }

    //selected building
    public Building buildingSelected;

    //number of battles occuring
    public List<TurnBasedBattleController> battles { get; private set; }

    //terrain mask
    private int terrainMask;

    void Awake()
    {
        battles = new List<TurnBasedBattleController>();
        groups = new List<Group>();
        selectedGroup = null;
        terrainMask = LayerMask.GetMask("TerrainLayer");
    }

    void Update()
    {
        //do input checks here
        // Shoot a raycast at the mouse position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Physics.Raycast(ray, out hit, float.MaxValue, terrainMask);
        Physics.Raycast(ray, out hit);

        // Move building with cursor if a building is currently selected (keep it on the terrain)
        if (buildingSelected != null) { buildingSelected.MoveBuilding(hit.point); }

        //check for input
        InputCheck(hit);
    }

    void BattleUpdate()
    {
        for (int i = 0; i < battles.Count; i++)
        {
            TurnBasedBattleController tbbc = battles[i];

            if (tbbc.battleOver)
            {
                bool playersWon = true;
                tbbc.enemyGroup.BattledEnded();
                for (int j = 0; i < tbbc.enemyGroup.GetUnits().Count; i++)
                {
                    if (!tbbc.enemyGroup.GetUnits()[j].IsDead)
                        playersWon = false;
                }
                tbbc.playerGroup.BattledEnded();
                for (int j = 0; i < tbbc.playerGroup.GetUnits().Count; i++)
                {
                    if (playersWon)
                    {
                        //Give players rewards
                    }
                }
                tbbc.playerGroup.BattledEnded();

                Destroy(tbbc.arena);
                battles.Remove(tbbc);
                i--;
            }
            else
                tbbc.Update();
        }
    }

    //selecting deselecting a group
    void SelectGroup(Group myGroup)
    {
        //activate the selection gameObject
        /*
        for (int i = 0; i < myGroup.GetUnits().Count; i++)
        {
            myGroup.GetUnits()[i].GetSelectionObject().setActive(true);
        }
        */

        selectedGroup = myGroup;
    }

    //Deselects 
    void DeselectGroup()
    {
        //nothing is selected
        if (selectedGroup == null) return;

        //Deactivate the selection gameObject
        /*
        for (int i = 0; i < selectedGroup.GetUnits().Count; i++)
        {
            selectedGroup.GetUnits()[i].GetSelectionObject().setActive(false);
        }
        */

        selectedGroup = null;
    }

    // Check player Input
    void InputCheck(RaycastHit hit)
    {
        // TEMP Building Selection Code
        if (Input.GetKeyDown("1") && buildingSelected == null)
        {
            buildingSelected = Instantiate(guildHallPrefab, Vector3.zero, Quaternion.Euler(-90.0f, 0.0f, 0.0f)).GetComponent<GuildHallController>().building;
            buildingSelected.MoveBuilding(hit.point);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (groups.Count > 0)
                selectedGroup = groups[(groups.IndexOf(selectedGroup) + 1) % groups.Count];
        }

        // Left click input 
        // Check if the player issued a command by clicking and act accordingly
        if (Input.GetMouseButtonDown(0))
        {
            if (buildingSelected != null)
            {
                if (buildingSelected.PlaceBuildingOnTerrain())
                {
                    buildingSelected = null;
                    Debug.Log("Building Placed");
                }

                selectedGroup = null;
            }
            //else if (hit.collider.gameObject.name == "Terrain")
            //{
            //    Debug.Log("clicked terrain deselect everything");
             //   selectedGroup = null;
            //}
            // Check if the player is placing a buildin
            else //search for a group
            {
                //Debug.Log(hit.collider.gameObject.name);
                //check if we are clicking this group so check any of the units being selected 
                for (int i = 0; i < groups.Count; i++)
                {
                    for (int j = 0; j < groups[i].GetUnits().Count; j++)
                    {
                        if (hit.collider.gameObject == groups[i].GetUnits()[j].GetGameObject())
                        {
                            selectedGroup = groups[i];
                            Debug.Log("selected a group");
                            break;
                        }
                    }
                }
            }
        }

        // Right click input 
        // Check if the player is cancelling the command to place a building
        if (Input.GetMouseButtonDown(1))
        {
            if (buildingSelected != null) // Deselecting building
            {
                Destroy(buildingSelected.GameObject);
                buildingSelected = null;
                Debug.Log("Cancelling Building Placement");
            }
            else if (selectedGroup != null && !selectedGroup.GetUnits()[0].IsInBattle) // Move a group to a position 
            {
                Transform objectHit = hit.transform;
                selectedGroup.SetGroupDestination(hit.point);
                Debug.Log("Group is moving to " + hit.point.ToString());
            }
        }
    }



}