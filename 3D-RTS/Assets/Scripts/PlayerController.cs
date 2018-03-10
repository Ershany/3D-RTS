using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //prefabs
    public GameObject guildHallPrefab;
    public GameObject arenaPrefab;

    //selected group of units
    public Group SelectedGroup { get; private set; }

    //players groups 
    public List<Group> groups { get; private set; }

    //selected building
    private Building buildingSelected;

    //number of battles occuring
    public List<TurnBasedBattleController> battles { get; private set; }

    //terrain mask
    private int terrainMask;

    void Awake()
    {
        battles = new List<TurnBasedBattleController>();
        groups = new List<Group>();
        SelectedGroup = null;
        terrainMask = LayerMask.GetMask("TerrainLayer");
    }

    void Update()
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (groups.Count > 0)
                SelectedGroup = groups[(groups.IndexOf(SelectedGroup) + 1) % groups.Count];
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
                if (!SelectedGroup.GetUnits()[0].IsInBattle)
                {
                    Transform objectHit = hit.transform;
                    SelectedGroup.SetGroupDestination(hit.point);
                    Debug.Log("Group is moving to " + hit.point.ToString());
                }

            }
        }

        // Check if the player is cancelling the command to place a building
        if (Input.GetMouseButtonDown(1) && buildingSelected != null)
        {
            Destroy(buildingSelected.GameObject);
            buildingSelected = null;
            Debug.Log("Cancelling Building Placement");
        }

        /*
        for (int i = 0; i < 1; i++)
        {
            if (groups[0].GetUnits()[i].IsInBattle)
                continue;

            for (int j = 0; j < 1; j++)
            {
                if (groups[1].GetUnits()[j].IsInBattle)
                    continue;

                if (Vector3.Distance(groups[0].GetUnits()[i].GetTransform().position, groups[1].GetUnits()[j].GetTransform().position) < 4)
                {
                    GameObject arena = Instantiate(arenaPrefab, groups[1].GetUnits()[i].GetTransform().position, Quaternion.identity);
                    battles.Add(new TurnBasedBattleController(new Vector3(groups[1].GetUnits()[i].GetTransform().position.x, 0.0f, groups[1].GetUnits()[i].GetTransform().position.z), groups[0], groups[1], arena));
                    i = 4;
                    groups[0].BattleStarted();
                    groups[1].BattleStarted();
                    break;
                }
            }  
        }
        */

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



    public void AddGroup(Group group)
    {
        groups.Add(group);
        SelectedGroup = group;
    }
}