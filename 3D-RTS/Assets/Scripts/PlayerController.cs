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
    
    //box selection
    public GameObject selectionPanel;

    //check whether a guild hall is built
    public bool guildHallBuilt;    //I think we are only allowed to have one guild hall 

    //behavior util file
    public BehaviorUtil behavior;

    //???
    public Vector3 markerPoint;

    int terrainMask;

    void Awake()
    {
        behavior = new BehaviorUtil();
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
        RaycastHit terrainHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //get terrain layer 
        terrainMask = LayerMask.GetMask("TerrainLayer");

        //raycasts here
        Physics.Raycast(ray, out terrainHit, float.MaxValue, terrainMask);
        Physics.Raycast(ray, out hit);

        //???????????????
        Plane p = new Plane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 500.0f));
        float aa;
        p.Raycast(ray, out aa);

        Vector3 intersectPoint = ray.GetPoint(aa);

        //Physics.Raycast(ray, out moveHit, 800.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        //Physics.Raycast(ray, out hit, 800.0f, 1 << 0);

        //Behaviors();

        if (hit.collider != null)
        {
            // Move building with cursor if a building is currently selected (keep it on the terrain)
            if (buildingToBeBuilt != null)
            {
                buildingToBeBuilt.MoveBuilding(terrainHit.point);
            }

            //check for input
            InputCheck(hit, terrainHit);

        }

        //check for battles
        BattleUpdate();
    }

        /*
        if (intersectPoint.x > 0 && intersectPoint.x < 500 &&
            intersectPoint.z > 0 && intersectPoint.z < 500 && intersectPoint.y == 0)
        {
            // Move building with cursor if a building is currently selected (keep it on the terrain)
            if (buildingToBeBuilt != null)
            {
                buildingToBeBuilt.MoveBuilding(terrainHit.point);
            }

            //check for input
            InputCheck(hit, terrainHit);

        }
        //check for battles
        BattleUpdate();
    }
    */

    void Behaviors()
    {
        //lerping with ease in ease out
        /*
        if (selectedGroup != null)
        {
            //fix t parameter for lerp function
            //t += (float) 0.1 * Time.deltaTime;

            for (int i = 0; i < selectedGroup.GetUnits().Count; i++)
            {
               
                //
                //Debug.Log("Destination: " + selectedGroup.GetUnits()[i].destination);
                //Debug.Log("Lerp result: " + BehaviorUtil.Lerp(selectedGroup.GetUnits()[i].GetTransform().position, selectedGroup.GetUnits()[i].destination, t));

                //selectedGroup.GetUnits()[i].GetAgent().velocity = BehaviorUtil.Lerp(selectedGroup.GetUnits()[i].GetTransform().position , selectedGroup.GetUnits()[i].destination , t) - selectedGroup.GetUnits()[i].GetTransform().position;                
            }

            if (t >= 1) t = 0;
        }
        */

        //seeking behavior Needs to be implemented here 
        /*
        if (selectedGroup != null)
        {
            //fix t parameter for lerp function
            //t += (float) 0.1 * Time.deltaTime;

            for (int i = 0; i < selectedGroup.GetUnits().Count; i++)
            {

                //
                //Debug.Log("Destination: " + selectedGroup.GetUnits()[i].destination);
                //Debug.Log("Lerp result: " + BehaviorUtil.Lerp(selectedGroup.GetUnits()[i].GetTransform().position, selectedGroup.GetUnits()[i].destination, t));

                //selectedGroup.GetUnits()[i].GetAgent().velocity = BehaviorUtil.Lerp(selectedGroup.GetUnits()[i].GetTransform().position , selectedGroup.GetUnits()[i].destination , t) - selectedGroup.GetUnits()[i].GetTransform().position;                
            }

            if (t >= 1) t = 0;
        }
        */

        //Wander behavior very wonky .....
        //behavior.Wander(groups);

        //flocking behavior
        //behavior.Flock(groups);
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

    //deselect all units buildings etc...
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
    void InputCheck(RaycastHit hit, RaycastHit terrainHit)
    {
        //Maybe have a unit killing button if they are stuck or smthg (delete button or smthg like that)

        /*
           *Building creation occurs here (1)
            THIS IS TEMPORARY
        */
        if (Input.GetKeyDown("1") && guildHallBuilt == false)
        {
            if (buildingToBeBuilt != null)
            {
                //Replace buildings if we already have a building to build
                Destroy(buildingToBeBuilt.GameObject);
                buildingToBeBuilt = null;
            }

            buildingToBeBuilt = Instantiate(guildHallPrefab, Vector3.zero, Quaternion.Euler(-90.0f, 0.0f, 0.0f)).GetComponent<GuildHallController>().building;
            buildingToBeBuilt.MoveBuilding(terrainHit.point);
        }

        /*
            *Hotkey for selecting units (TabKey)
        */
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (groups.Count > 0) { selectedGroup = groups[(groups.IndexOf(selectedGroup) + 1) % groups.Count]; }
        }
        
        /*
            Left mouse click:
            *Place buildings
            *Select Units
            *Select Building ????
        */
        if (Input.GetMouseButtonDown(0))
        {
            if (buildingToBeBuilt != null)
            {
                // Check what type of building we are making when we do this 
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
            else
            {
                //check if we are clicking this group so check any of the units being selected 
                for (int i = 0; i < groups.Count; i++)
                {
                    Debug.Log("started searching groups");
                    for (int j = 0; j < groups[i].GetUnits().Count; j++)
                    {
                        Debug.Log("searching units of group " + i);
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject == groups[i].GetUnits()[j].GetGameObject())
                            {
                                Debug.Log("found the selected group");
                                selectedGroup = groups[i];
                                Debug.Log("selected a group");
                                break;
                            }
                        }
                    }
                }
            }
        }

        /*
            Right Click:
           *Select a destination for a selected group/s to go to 
           *Cancel a building selection
        */
        if (Input.GetMouseButtonDown(1))
        {
            if (buildingToBeBuilt != null)
            {
                //Deselecting building 
                if (!buildingToBeBuilt.IsPlaced)
                {
                    //check if building has already been placed if it hasn't destroy it
                    Destroy(buildingToBeBuilt.GameObject);
                    buildingToBeBuilt = null;
                    Debug.Log("Cancelling Building Placement");
                }
                else
                {
                    buildingSelected = null;
                    Debug.Log("Cancelling building related selection");
                }
            }
            else if (selectedGroup != null && !selectedGroup.GetUnits()[0].IsInBattle)
            {
                //Move a group to a position 
                selectedGroup.SetGroupDestination(terrainHit.point);
                
                Debug.Log("Group is moving to " + terrainHit.point.ToString());
            }
        }
    }



}