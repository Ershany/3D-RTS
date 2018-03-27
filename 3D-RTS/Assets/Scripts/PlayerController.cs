using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //prefabs
    public GameObject guildHallPrefab;
    public GameObject arenaPrefab;

    //selected group of units
    public List<Group> selectedGroups;

    //players groups 
    public List<Group> groups { get; private set; }

    //selected buildings 
    public Building buildingSelected;
    public Building buildingToBeBuilt;

    //number of battles occuring
    public List<TurnBasedBattleController> battles { get; private set; }

    //player buildings
    public List<Building> playerBuildings;
    
    //box selection
    public GameObject selectionPanel;

    //check whether a guild hall is built
    public bool guildHallBuilt;    //I think we are only allowed to have one guild hall 

    //booleans for player selection state 
    public bool playerSelectedGroups;
    public bool playerSelectedSingleGroup;
    public bool playerSelectedGuildHall;

    //behavior util file
    public BehaviorUtil behavior;

    //marker for unit destination
    public GameObject markerPrefab;
    public MarkerController destinationMarker;

    //particle systems for now here for testing purposes
    public GameObject spellPrefab;
    public GameObject slashPrefab;
    public GameObject hitPrefab;

    public SpellController magicSpell;
    public MarkerController magicSpell1; //TEMP FOR DEBUGGING
    public MarkerController swordSlash;
    public MarkerController unitHit;

    //used for terrain movements and selections
    int terrainMask;

    void Awake()
    {
        playerSelectedGroups = false;
        playerSelectedSingleGroup = false;
        playerSelectedGuildHall = false;
        selectedGroups = new List<Group>();
        behavior = new BehaviorUtil();
        guildHallBuilt = false;
        battles = new List<TurnBasedBattleController>();
        groups = new List<Group>();
        destinationMarker = Instantiate(markerPrefab , Vector3.zero , Quaternion.identity).GetComponent<MarkerController>();
        magicSpell1 = Instantiate(spellPrefab , Vector3.zero , Quaternion.identity).GetComponent<MarkerController>();
        swordSlash = Instantiate(slashPrefab , Vector3.zero, Quaternion.identity).GetComponent<MarkerController>();
        unitHit = Instantiate(hitPrefab , Vector3.zero , Quaternion.identity).GetComponent<MarkerController>();
    }

    void Update()
    {
        // Shoot raycasts at the mouse position
        RaycastHit hit;
        RaycastHit terrainHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //get terrain layer 
        terrainMask = LayerMask.GetMask("TerrainLayer");

        //raycasts here
        Physics.Raycast(ray, out terrainHit, float.MaxValue, terrainMask);
        Physics.Raycast(ray, out hit);
        Plane p = new Plane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 500.0f));
        float aa;
        p.Raycast(ray, out aa);
        Vector3 intersectPoint = ray.GetPoint(aa);

        if (intersectPoint.x > 0 && intersectPoint.x < 500 &&
            intersectPoint.z > 0 && intersectPoint.z < 500)
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

    void Behaviors()
    {
        
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

            buildingToBeBuilt = Instantiate(guildHallPrefab, Vector3.zero, Quaternion.Euler(-90.0f, -45.0f, 0.0f)).GetComponent<GuildHallController>().building;
            buildingToBeBuilt.MoveBuilding(terrainHit.point);
        }

        /*
            *HOTKEY FOR SELECTING UNITS (TabKey)
        */
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (groups.Count > 0)
            {
                //selectedGroups
                //selectedGroups.Clear();
                //selectedGroups = groups[(groups.IndexOf(selectedGroup) + 1) % groups.Count];
            }
        }
        
        /*
            LEFT MOUSE CLICK:
            *Place buildings
            *Select Units
            *Select Building ????
            *Deselect all things when we click on terrain
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
                    SelectBuilding(buildingToBeBuilt);
                    guildHallBuilt = true;
                    buildingToBeBuilt = null;
                    Debug.Log("Building Placed");
                }

                DeselectGroups();
            }
            else if (hit.collider.gameObject.name == "Terrain")
            {
                Deselect();
                Debug.Log("clicked terrain deselect buildingToBeBuilt");
            }
            else if (hit.collider.gameObject != null)
            {
                if (hit.collider.gameObject.GetComponent<GuildHallController>() != null)
                {
                    SelectBuilding(hit.collider.gameObject.GetComponent<GuildHallController>().building);
                    DeselectGroups();
                    playerSelectedSingleGroup = false;
                    playerSelectedGroups = false;
                    playerSelectedGuildHall = true;
                }
                else
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        Debug.Log("started searching groups");

                        for (int j = 0; j < groups[i].GetUnits().Count; j++)
                        {
                            Debug.Log("searching units of group " + i);

                            if (hit.collider.gameObject == groups[i].GetUnits()[j].GetGameObject())
                            {
                                DeselectGroups();
                                selectedGroups.Add(groups[i]);
                                SelectGroups();
                                playerSelectedSingleGroup = true;

                                DeselectBuilding();
                                buildingToBeBuilt = null;
                                Debug.Log("found the selected group");
                                break;
                            }
                        }
                    }
                }
            }
        }

        /*
            RIGHT CLICK:
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
                    // no need for this anymore
                    DeselectBuilding();
                    Debug.Log("Cancelling building related selection");
                }
            }
            else if (selectedGroups.Count > 0)
            {
                //Move a groups to a position 
                for (int i = 0; i < selectedGroups.Count; i++)
                {
                    if (!selectedGroups[i].GetFirstUnit().IsInBattle)
                    {
                        selectedGroups[i].SetGroupDestination(terrainHit.point);
                    }
                }

                Vector3 destination = terrainHit.point + new Vector3(0, selectedGroups[0].GetFirstUnit().GetTransform().position.y + 5.0f, 0);

                //do destination marker code here (might need some animation etc...)
                //destinationMarker.ActivateMarker(destination);
                //swordSlash.ActivateMarker(destination);
                //magicSpell1.ActivateMarker(destination);
                unitHit.ActivateMarker(destination);

                Debug.Log("Group is moving to " + terrainHit.point.ToString());
            }
        }
    }
    
    public void SelectOnRect(Vector2 v1, Vector2 v2)
    {
        Vector3 point1, point2;


        Debug.Log("Here1");
        RaycastHit hit1;
        RaycastHit hit2;
        Ray ray1 = Camera.main.ScreenPointToRay(v1);
        Ray ray2 = Camera.main.ScreenPointToRay(v2);

        Physics.Raycast(ray1, out hit1, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        Physics.Raycast(ray2, out hit2, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);

        Plane p = new Plane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 0.0f), new Vector3(500.0f, 0.0f, 500.0f));
        float aa;

        if (hit1.collider == null)
        {
            p.Raycast(ray1, out aa);
            point1 = ray1.GetPoint(aa);
        }
        else
        {
            point1 = hit1.point;
        }
        if (hit2.collider == null)
        {
            p.Raycast(ray2, out aa);
            point2 = ray2.GetPoint(aa);
        }
        else
        {
            point2 = hit2.point;
        }

        if (Input.GetKey(KeyCode.Semicolon))
        {

            if (GameObject.FindGameObjectsWithTag("tempTracker").Length > 0)
                Destroy(GameObject.FindGameObjectWithTag("tempTracker"));

            GameObject temp = Instantiate(arenaPrefab);

            Vector3 center = point1 - ((Vector3.Distance(point1, point2) * 0.5f) * Vector3.Normalize((point1 - point2))) + new Vector3(0, 1, 0);

            temp.tag = "tempTracker";

            temp.transform.position = center;

            temp.transform.localScale = new Vector3(Mathf.Abs(point1.x - point2.x), 1, Mathf.Abs(point1.z - point2.z));
        }
        
        if (point1 != null && point2 != null) {
            float minX = Mathf.Min(point1.x, point2.x);
            float maxX = Mathf.Max(point1.x, point2.x);
            float minZ = Mathf.Min(point1.z, point2.z);
            float maxZ = Mathf.Max(point1.z, point2.z);

            List<Group> newSelectedGroups = new List<Group>();

            for (int i = 0; i<groups.Count; i++)
            {
                List<DynamicUnit> _units = groups[i].GetUnits();
                for (int j = 0; j<_units.Count; j++)
                {
                    Vector3 unitPos = _units[j].GetTransform().position;
                    //Debug.Log("Unit Number " + j.ToString() + " " + unitPos + " " + minX + " " + maxX + ")
                    if (unitPos.x > minX &&
                        unitPos.x < maxX &&
                        unitPos.z > minZ &&
                        unitPos.z < maxZ)
                    {
                        newSelectedGroups.Add(groups[i]);
                        j = _units.Count;
                    }             
                }
            }

            if (newSelectedGroups.Count > 0)
            {
                DeselectGroups();
                selectedGroups = newSelectedGroups;
                SelectGroups();

                DeselectBuilding();
                if (selectedGroups.Count == 1)
                {
                    Debug.Log("HereSingle");
                    playerSelectedSingleGroup = true;
                }
                else
                {
                    Debug.Log("HereMany");
                    playerSelectedGroups = true;
                }
            }
        }
    }

    //selects a building
    public void SelectBuilding(Building building)
    {
        buildingSelected = building;
        HighlightBuilding(building, true);
    }

    //selects a group
    void SelectGroups()
    {
        foreach (Group group in selectedGroups)
        {
            HighlightGroup(group, true);
        }
    }

    //deslects all groups
    void DeselectGroups()
    {
        foreach (Group group in selectedGroups)
        {
            HighlightGroup(group, false);
        }
        selectedGroups.Clear();
    }

    //deslect buildings
    void DeselectBuilding()
    {
        if (buildingSelected != null)
        {
            HighlightBuilding(buildingSelected, false);
            buildingSelected = null;
        }
    }

    //hightlight code for groups
    void HighlightGroup(Group group, bool shouldHighlight)
    {
        // Loop through the group and activate/deactivate the highlight quad for each unit
        for (int i = 0; i < group.GetUnits().Count; ++i)
        {
            GameObject gameObj = group.GetUnits()[i].GetGameObject();
            foreach (Transform child in gameObj.transform)
            {
                if (child.CompareTag("HighlightQuad"))
                {
                    child.gameObject.SetActive(shouldHighlight);
                }
            }
        }
    }

    //hightlight code for buildings
    void HighlightBuilding(Building building, bool shouldHighlight)
    {
        foreach (Transform child in building.GameObject.transform)
        {
            if (child.CompareTag("HighlightQuad"))
            {
                child.gameObject.SetActive(shouldHighlight);
            }
        }
    }

    //deselect everything
    public void Deselect()
    {
        DeselectBuilding();
        DeselectGroups();
    }
}