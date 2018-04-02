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

    // states
    private static OffenseState offenseState;
    private static DefenseState defenseState;
    private static HarvestState harvestState;

    void Awake()
    {
        enemyGroups = new List<Group>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        State = offenseState;
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
