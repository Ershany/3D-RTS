using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // members
    public List<Group> enemyGroups;
    private Group strongestGroup;
    private Group selectedGroup { get; set; }
    public IState State { get; set; }

    // references
    public GameController gameController;
    public PlayerController playerController;

    //maybe handle enemy units and buildings here
    public List<Building> enemyBuildings;

    // states
    public static OffenseState offenseState = new OffenseState();
    public static DefenseState defenseState = new DefenseState();
    public static HarvestState harvestState = new HarvestState();

    void Awake()
    {
        enemyBuildings = new List<Building>();
        enemyGroups = new List<Group>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        State = harvestState;
    }

    void Update()
    {
        State?.Update(this);
    }

    public void AddGroup(Group group)
    {
        enemyGroups.Add(group);
        
        if (strongestGroup == null)
        {
            strongestGroup = group;
        }
        else
        {
            
        }
    }

    public bool RemoveGroup(Group group) 
    {
        return enemyGroups.Remove(group);
    }
}
