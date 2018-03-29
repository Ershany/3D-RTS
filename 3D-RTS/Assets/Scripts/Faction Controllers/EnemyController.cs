using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //enemy units
    public List<Group> enemyGroups;
    public Group selectedGroup { get; set; }

    //game controller
    public GameController gameController;

    //maybe handle enemy units and buildings here
    public List<Building> enemyBuildings;

    void Awake()
    {
        enemyBuildings = new List<Building>();
        enemyGroups = new List<Group>();

        //Reference Game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        //do stuff
        //mostly ai stuff all enemy moves will be made here
    }
}
