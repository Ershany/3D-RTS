using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //GET RID OF THAT SHIT
    /* Prefabs */
    public GameObject enemyArcherPrefab;
    public GameObject enemyInfantryPrefab;
    public GameObject enemyMagePrefab;
    public GameObject groupPrefab;

    //enemy units
    public List<Group> enemyGroups { get; private set; }
    public Group selectedGroup { get; set; }

    //game controller
    public GameController gameController;

    //maybe handle enemy units and buildings here

    void Awake()
    {
       //Reference Game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //do this in a function !!!! in game controller 
        EnemyArcherController archer = Instantiate(enemyArcherPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry = Instantiate(enemyInfantryPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage = Instantiate(enemyMagePrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        enemyGroups = new List<Group>();

        //make a group of enemies
        Group enemyGroup = Instantiate(groupPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponent<Group>();
        enemyGroup.AddUnit(archer.demonUnit);
        enemyGroup.AddUnit(infantry.demonUnit);
        enemyGroup.AddUnit(mage.demonUnit);

        //add this group to the array 
        enemyGroups.Add(enemyGroup);
        enemyGroups[0].SetGroupDestination(new Vector3(20, 0, 50));
    }

    // Update is called once per frame
    void Update()
    {

        //do stuff
        //mostly ai stuff all enemy moves will be made here
    }
}
