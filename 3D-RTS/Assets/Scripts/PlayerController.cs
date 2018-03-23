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

    //selected buildings 
    public Building buildingSelected;
    public Building buildingToBeBuilt;

    //number of battles occuring
    public List<TurnBasedBattleController> battles { get; private set; }
    public List<Building> playerBuildings;

    public GameObject selectionPanel;

    bool guildHallBuilt;


    public Vector3 markerPoint;

    void Awake()
    {
        guildHallBuilt = false;
        battles = new List<TurnBasedBattleController>();
        groups = new List<Group>();
        selectedGroup = null;
        markerPoint = new Vector3(-1, 0, 0);
    }

    void Update()
    {
        // do input checks here
        // Shoot a raycast at the mouse position
        RaycastHit hit;
        RaycastHit moveHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Physics.Raycast(ray, out hit, float.MaxValue, terrainMask);

        Plane p = new Plane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 500.0f));
        float aa;
        p.Raycast(ray, out aa);

        Vector3 intersectPoint = ray.GetPoint(aa);
        Physics.Raycast(ray, out hit, 800.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        Physics.Raycast(ray, out moveHit, 800.0f, 1 << 0);
        /*
            BehaviorUtil.Flock(groups);
        */
        if (intersectPoint.x > 0 && intersectPoint.x < 500 &&
            intersectPoint.z > 0 && intersectPoint.z < 500 && intersectPoint.y == 0)
        {



            // Move building with cursor if a building is currently selected (keep it on the terrain)
            if (buildingToBeBuilt != null)
            {
                buildingToBeBuilt.MoveBuilding(moveHit.point);
            }

            //check for input
            InputCheck(moveHit, hit);

        }
        //check for battles
        BattleUpdate();
    }

    //Check for battles
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
            {
                tbbc.Update();
            }
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

    public void Deselect()
    {
        //deselect everything

        if (selectedGroup != null)
        {
            //Deactivate the selection gameObject
            /*
            for (int i = 0; i < selectedGroup.GetUnits().Count; i++)
            {
                selectedGroup.GetUnits()[i].GetSelectionObject().setActive(false);
            }
            */

            selectedGroup = null;
        }

        if (buildingSelected != null)
        {
            //deactivate selection gameObject
            buildingSelected = null;
        }

        buildingToBeBuilt = null;
    }

    //Check player Input
    void InputCheck(RaycastHit hit, RaycastHit moveHit)
    {

        //CHANGE HEREEEEEEEEEE
        // TEMP Building Selection Code
        if (Input.GetKeyDown("1") && guildHallBuilt == false)
        {
            if (buildingToBeBuilt != null)
            {
                //Replace buildings if we already have a building to build
                Destroy(buildingToBeBuilt.GameObject);
                buildingToBeBuilt = null;
            }

            buildingToBeBuilt = Instantiate(guildHallPrefab, Vector3.zero, Quaternion.Euler(-90.0f, 0.0f, 0.0f)).GetComponent<GuildHallController>().building;
            buildingToBeBuilt.MoveBuilding(moveHit.point);
        }

        // some group selection code
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (groups.Count > 0) { selectedGroup = groups[(groups.IndexOf(selectedGroup) + 1) % groups.Count]; }
        }

        //CHANGEEE HEREEEEEE
        // Left click input 
        // Check if the player issued a command by clicking and act accordingly
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Check1");
            if (buildingToBeBuilt != null)
            {
                // Place the building on the terrain
                if (buildingToBeBuilt.PlaceBuildingOnTerrain())
                {
                    buildingToBeBuilt.GameObject.GetComponent<GuildHallController>().GuildHallPlaced();
                    guildHallBuilt = true;
                    buildingToBeBuilt = null;
                    Debug.Log("Building Placed");
                }

                //Just in case
                buildingSelected = null;
                selectedGroup = null;
            }
            //else if (hit.collider.gameObject.name == "Terrain")
            //{
            //   selectedGroup = null;
            //   buildingSelected = null;
            //   buildingToBeSelected = null;
            //   Debug.Log("clicked terrain deselect everything");
            //}
            else //search for a group
            {
                //Debug.Log(hit.collider.gameObject.name);
                //check if we are clicking this group so check any of the units being selected 
                for (int i = 0; i < groups.Count; i++)
                {
                    Debug.Log("Check2");
                    for (int j = 0; j < groups[i].GetUnits().Count; j++)
                    {

                        Debug.Log("Check3");
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject == groups[i].GetUnits()[j].GetGameObject())
                            {
                                Debug.Log("Check4");
                                selectedGroup = groups[i];
                                Debug.Log("selected a group");
                                break;
                            }
                        }
                    }
                }
            }
        }

        //CHANGEEEE HERREEEEEE
        // Right click input 
        // Check if the player is cancelling the command to place a building
        if (Input.GetMouseButtonDown(1))
        {
            if (buildingToBeBuilt != null)
            {
                // Deselecting building 
                if (!buildingToBeBuilt.IsPlaced)
                {
                    //check if building has already been placed if it hasn't destroy it
                    Destroy(buildingToBeBuilt.GameObject);
                    buildingToBeBuilt = null;
                    Debug.Log("Cancelling Building Placement");
                }

                buildingSelected = null;
                Debug.Log("Cancelling building related selection");
            }
            else if (selectedGroup != null && !selectedGroup.GetUnits()[0].IsInBattle)
            {
                // Move a group to a position 
                selectedGroup.SetGroupDestination(moveHit.point);
                
                Debug.Log("Group is moving to " + moveHit.point.ToString());
            }
        }
    }



}