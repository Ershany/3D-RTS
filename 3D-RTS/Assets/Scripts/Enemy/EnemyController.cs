using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // members
    public List<Group> enemyGroups;
    private Group selectedGroup { get; set; }
    public IState State { get; set; }

    // references
    public GameController gameController;
    public PlayerController playerController;

    //maybe handle enemy units and buildings here
    public List<Building> enemyBuildings;

    // states
    public static OffenseState offenseState;
    public static DefenseState defenseState;
    public static HarvestState harvestState;

    void Awake()
    {
        offenseState = new OffenseState(this.gameObject);
        defenseState = new DefenseState(this.gameObject);
        harvestState = new HarvestState(this.gameObject);

        
        enemyBuildings = new List<Building>();
        enemyGroups = new List<Group>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        State = defenseState;
        defenseState.SetDefenseTime(10.0f);
    }

    void Update()
    {
        State?.Update();
    }

    public void AddGroup(Group group)
    {
        enemyGroups.Add(group);
    }

    public bool RemoveGroup(Group group) 
    {
        return enemyGroups.Remove(group);
    }
}
