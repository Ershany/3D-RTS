using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //prefabs
    public GameObject guildHallPrefab;
    public GameObject arenaPrefab;
    public GameObject markerPrefab;
    public GameObject spellPrefab;
    public GameObject slashPrefab;
    public GameObject hitPrefab;

    //references
    public GameController gameController;

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
    public bool guildHallBuilt;

    //booleans for player selection state 
    public bool playerSelectedGroups;
    public bool playerSelectedSingleGroup;
    public bool playerSelectedGuildHall;

    //behavior util file
    public BehaviorUtil behavior;

    //controllers for the particle systems
    public ProjectileController magicSpell;
    public EffectController swordSlash;
    public EffectController unitHit;
    public EffectController destinationMarker;

    //not really sure
    private string activeCommand;
    private Vector3 patrolStartPoint;

    //used for terrain movements and selections
    int terrainMask;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        patrolStartPoint = new Vector3(-1, -1, -1);
        playerSelectedGroups = false;
        playerSelectedSingleGroup = false;
        playerSelectedGuildHall = false;
        guildHallBuilt = false;
        selectedGroups = new List<Group>();
        battles = new List<TurnBasedBattleController>();
        groups = new List<Group>();
        playerBuildings = new List<Building>();
        behavior = new BehaviorUtil();

        destinationMarker = Instantiate(markerPrefab , Vector3.zero , Quaternion.identity).GetComponent<EffectController>();
        magicSpell = Instantiate(spellPrefab , Vector3.zero , Quaternion.identity).GetComponent<ProjectileController>();
        swordSlash = Instantiate(slashPrefab , Vector3.zero, Quaternion.identity).GetComponent<EffectController>();
        unitHit = Instantiate(hitPrefab , Vector3.zero , Quaternion.identity).GetComponent<EffectController>();

        //set explosion reference
        magicSpell.explosion = unitHit;
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

        if (hit.collider)
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
        ErrorCorrection();
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
                if (tbbc.enemyGroup != null)
                {


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



                    for (int j = 0; i < tbbc.randomBattleEnemies.Count; i++)
                    {
                        if (tbbc.randomBattleEnemies[j].IsDead)
                            playersWon = false;
                        Destroy(tbbc.randomBattleEnemies[j].GetGameObject());
                        j--;
                    }


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

        //build blacksmith
        if (Input.GetKeyDown("2"))
        {
            if (buildingToBeBuilt != null)
            {
                //Replace buildings if we already have a building to build
                Destroy(buildingToBeBuilt.GameObject);
                buildingToBeBuilt = null;
            }

            buildingToBeBuilt = gameController.CreateBlacksmith(true);
            buildingToBeBuilt.MoveBuilding(terrainHit.point);
        }

        //build Archery range
        if (Input.GetKeyDown("3"))
        {
            if (buildingToBeBuilt != null)
            {
                //Replace buildings if we already have a building to build
                Destroy(buildingToBeBuilt.GameObject);
                buildingToBeBuilt = null;
            }

            buildingToBeBuilt = gameController.CreateArcheryRange(true);
            buildingToBeBuilt.MoveBuilding(terrainHit.point);
        }

        //build temple of magi
        if (Input.GetKeyDown("4"))
        {
            if (buildingToBeBuilt != null)
            {
                //Replace buildings if we already have a building to build
                Destroy(buildingToBeBuilt.GameObject);
                buildingToBeBuilt = null;
            }

            buildingToBeBuilt = gameController.CreateTempleOfMagi(true);
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
                    //DeselectBuilding();
                    //DeselectGroups();
                    Deselect();
                    SelectBuilding(buildingToBeBuilt);

                    if (buildingToBeBuilt.Type == Building.BuildingType.GUILDHALL)
                    {
                        guildHallBuilt = true;
                        buildingToBeBuilt.GameObject.GetComponent<GuildHallController>().GuildHallPlaced();
                    }

                    buildingToBeBuilt = null;
                    Debug.Log("Building Placed");
                }
            }
            else if (hit.collider.gameObject.name == "Terrain")
            {
                switch (activeCommand)
                {
                    case "Move":
                        if (selectedGroups.Count > 0)
                            SetSelectedUnitsDestination(terrainHit.point, null);
                        break;
                    case "Attack":

                        break;
                    case "Patrol":
                        if (patrolStartPoint.x != -1)
                        {
                            SetSelectedUnitsPatrol(patrolStartPoint, terrainHit.point);
                        }
                        else
                        {
                            patrolStartPoint = terrainHit.point;
                        }
                        break;

                        //Deselect();
                        //Debug.Log("clicked terrain deselect buildingToBeBuilt");
                        //break;
                }

            }
            else if (hit.collider.gameObject != null)
            {

                switch (activeCommand)
                {
                    case "Move":
                        if (selectedGroups.Count > 0)
                            SetSelectedUnitsDestination(terrainHit.point, hit.collider.gameObject.transform);
                        break;
                    case "Attack":

                        break;
                    case "Patrol":
                        if (patrolStartPoint.x != -1)
                        {
                            SetSelectedUnitsPatrol(patrolStartPoint, terrainHit.point);
                        }
                        else
                        {
                            patrolStartPoint = terrainHit.point;
                        }
                        break;

                    default:
                        if (hit.collider.gameObject.GetComponent<GuildHallController>() != null)
                        {
                            Deselect();
                            SelectBuilding(hit.collider.gameObject.GetComponent<GuildHallController>().building);
                            playerSelectedSingleGroup = false;
                            playerSelectedGroups = false;
                            playerSelectedGuildHall = true;
                        }
                        else if (hit.collider.gameObject.GetComponent<TechnologyBuildingController>())
                        {
                            Deselect(); 
                            SelectBuilding(hit.collider.gameObject.GetComponent<TechnologyBuildingController>().building);
                            playerSelectedSingleGroup = false;
                            playerSelectedGroups = false;
                            playerSelectedGuildHall = false;
                        }
                        else
                        {
                            for (int i = 0; i < groups.Count; i++)
                            {
                                //Debug.Log("started searching groups");

                                for (int j = 0; j < groups[i].GetUnits().Count; j++)
                                {
                                    //Debug.Log("searching units of group " + i);

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
                        break;
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
                SetSelectedUnitsDestination(terrainHit.point, null);

                destinationMarker.ActivateMarker(selectedGroups[0].GetUnits()[0].GetAgent().destination + new Vector3 (0 , 0.5f, 0));

                //do destination marker code here (might need some animation etc...)
                //destinationMarker.ActivateMarker(destination);
                //swordSlash.ActivateMarker(destination);
                //unitHit.ActivateMarker(destination);
                //magicSpell.Activate(selectedGroups[0].GetFirstUnit().GetTransform().position + new Vector3 (0, 5.0f ,0) , destination);

                Debug.Log("Group is moving to " + terrainHit.point.ToString());
            }
        }
    }

    private void SetSelectedUnitsDestination(Vector3 destination, Transform dynamicDestination)
    {
        for (int i = 0; i < selectedGroups.Count; i++)
        {
            if (!selectedGroups[i].GetFirstUnit().IsInBattle)
            {
                selectedGroups[i].SetGroupDestination(destination, dynamicDestination);
            }
        }
        patrolStartPoint = new Vector3(-1, -1, -1);
    }

    private void SetSelectedUnitsPatrol(Vector3 destination1, Vector3 destination2)
    {
        for (int i = 0; i < selectedGroups.Count; i++)
        {
            if (!selectedGroups[i].GetFirstUnit().IsInBattle)
            {
                selectedGroups[i].SetGroupPatrol(destination1, destination2);
            }
        }
        patrolStartPoint = new Vector3(-1, -1, -1);
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

            Debug.Log("Here2");
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
                        unitPos.z < maxZ )
                    {
                        Debug.Log("Here3");
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

    public void SelectOnPoly(Vector2 v1, Vector2 v2)
    {
        Vector2 v12, v21, vn;
        Vector3 point1, point2, point3, point4, point5;

        v12 = new Vector2(v1.x, v2.y);
        v21 = new Vector2(v2.x, v1.y);
        vn = v1 - ((Vector2.Distance(v1, v2) * 0.5f) * new Vector2(Vector3.Normalize((v1 - v2)).x, Vector3.Normalize((v1 - v2)).y));

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        RaycastHit hit5;

        Ray ray1 = Camera.main.ScreenPointToRay(v1);
        Ray ray2 = Camera.main.ScreenPointToRay(v2);
        Ray ray3 = Camera.main.ScreenPointToRay(v12);
        Ray ray4 = Camera.main.ScreenPointToRay(v21);

        Physics.Raycast(ray1, out hit1, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        Physics.Raycast(ray2, out hit2, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        Physics.Raycast(ray3, out hit3, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);
        Physics.Raycast(ray4, out hit4, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);

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

        if (hit3.collider == null)
        {
            p.Raycast(ray3, out aa);
            point3 = ray3.GetPoint(aa);
        }
        else
        {
            point3 = hit3.point;
        }

        if (hit4.collider == null)
        {
            p.Raycast(ray4, out aa);
            point4 = ray4.GetPoint(aa);
        }
        else
        {
            point4 = hit4.point;
        }


        Ray ray5 = Camera.main.ScreenPointToRay(vn);
        Physics.Raycast(ray5, out hit5, 200.0f, 1 << GameObject.FindGameObjectWithTag("Terrain").layer);

        if (hit5.collider == null)
        {
            p.Raycast(ray5, out aa);
            point5 = ray5.GetPoint(aa);
        }
        else
        {
            point5 = hit5.point;
        }
        GameObject polyColGameObject = new GameObject();
        polyColGameObject.AddComponent<PolygonCollider2D>();
        PolygonCollider2D polyCollider = polyColGameObject.GetComponent<PolygonCollider2D>();
        Vector2[] ptsArr = new Vector2[4];

        ptsArr[0] = new Vector2 (point1.x, point1.z);
        ptsArr[1] = new Vector2(point2.x, point2.z);
        ptsArr[2] = new Vector2(point3.x, point3.z);
        ptsArr[3] = new Vector2(point4.x, point4.z);


        polyCollider.points = new Vector2[4];
        polyCollider.points = ptsArr;

        if (point1 != null && point2 != null)
        {
            
            List<Group> newSelectedGroups = new List<Group>();

            for (int i = 0; i < groups.Count; i++)
            {
                List<DynamicUnit> _units = groups[i].GetUnits();
                for (int j = 0; j < _units.Count; j++)
                {
                    Vector3 unitPos = _units[j].GetTransform().position;
                    //Debug.Log("Unit Number " + j.ToString() + " " + unitPos + " " + minX + " " + maxX + ")
                    if (polyCollider.bounds.Contains(new Vector2(unitPos.x, unitPos.z)))
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
        Destroy(polyColGameObject);

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
        patrolStartPoint = new Vector3(-1, -1, -1);
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

    public void SetActiveCommand(string command)
    {
        activeCommand = command;
    }

    public string GetActiveCommand()
    {
        return activeCommand;
    }

    //hightlight code for groups
    public void HighlightGroup(Group group, bool shouldHighlight)
    {
        // Loop through the group and activate/deactivate the highlight quad for each unit
        for (int i = 0; i < group.GetUnits().Count; ++i)
        {
            //if unit is dead he doesn't get highlighted anymore
            if (group.GetUnits()[i].IsDead && shouldHighlight) continue;

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

    void ErrorCorrection()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            for (int j = 0; j < groups[i].GetUnits().Count; j++)
            {
                if (groups[i].GetUnits()[j].GetGameObject().activeSelf)
                {
                    if (groups[i].GetUnits()[j].GetAgent().enabled == false)
                    {
                        Debug.Log("Reset agent for group " + i);
                        groups[i].GetUnits()[j].GetAgent().enabled = true;
                    }
                }
            }
        }
    }
}