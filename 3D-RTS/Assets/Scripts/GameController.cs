using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO LIST 
//add a string array for names
//work on guild hall
//play with unit creation 
//look at buildings
//Get Selection GameObject

public class GameController : MonoBehaviour
{
    /* Prefabs */   
    // player units
    public GameObject warriorPrefab;
    public GameObject roguePrefab;
    public GameObject magePrefab;

    // enemy units
    public GameObject enemyArcherPrefab;
    public GameObject enemyInfantryPrefab;
    public GameObject enemyMagePrefab;
    public GameObject groupPrefab;
    public GameObject arenaPrefab;

    /* References */
    private PlayerController playerController;
    private EnemyController enemyController;

    void Awake()
    {    
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();

        List<DynamicUnit> myUnits;
        myUnits = new List<DynamicUnit>();
        myUnits.Add(CreatePlayerArcher());
        myUnits.Add(CreatePlayerWarrior());
        myUnits.Add(CreatePlayerMage());

        // add some player units
        AddFactionGroup(myUnits , new Vector3 (20 ,0, 20) , true);  
    }

    void Update()
    {
        BattleCheck();
    }

    //Archer creation
    public FactionUnit CreatePlayerArcher()
    {
        return Instantiate(roguePrefab, new Vector3(30.0f, 2.0f, 30.0f), Quaternion.identity).GetComponent<RogueController>().unit;
    }

    //Warrior creation
    public FactionUnit CreatePlayerWarrior()
    {
        return Instantiate(warriorPrefab, new Vector3(30.0f, 2.0f, 30.0f), Quaternion.identity).GetComponent<WarriorController>().unit;
    }

    //mage creation
    public FactionUnit CreatePlayerMage()
    {
        return Instantiate(magePrefab, new Vector3(30.0f, 2.0f, 30.0f), Quaternion.identity).GetComponent<MageController>().unit;
    }

    //Enemy Archer creation
    public FactionUnit CreateEnemyArcher()
    {
        return Instantiate(enemyArcherPrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<EnemyArcherController>().demonUnit;
    }

    //Enemy Infantry creation
    public FactionUnit CreateEnemyWarrior()
    {
        return Instantiate(enemyInfantryPrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<EnemyInfantryController>().demonUnit;
    }

    //Enemy mage creation
    public FactionUnit CreateEnemyMage()
    {
        return Instantiate(enemyMagePrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<EnemyMageController>().demonUnit;
    }

    //Adding to a faction's grps
    public void AddFactionGroup(List<DynamicUnit> factionUnits , Vector3 position , bool isPlayer)
    {
        Group grp = Instantiate(groupPrefab, position, Quaternion.identity).GetComponent<Group>();

        for (int i = 0; i < factionUnits.Count; i++)
        {
            grp.AddUnit(factionUnits[i]);
        }

        // Add either to player or enemy
        if (isPlayer) { playerController.groups.Add(grp); }
        else { enemyController.enemyGroups.Add(grp); }
    }


    //Check for a battle instance 
    void BattleCheck()
    {
        //check for battles
        for (int i = 0; i < playerController.groups.Count; i++)
        {
            //if we a group is in a battle leave
            //might need to change this
            if (playerController.groups[i].GetFirstUnit().IsInBattle) continue;

            for (int j = 0; j < enemyController.enemyGroups.Count; j++)
            {
                if (Vector3.Distance(playerController.groups[i].GetUnits()[i].GetTransform().position, enemyController.enemyGroups[j].GetUnits()[j].GetTransform().position) < 4)
                {
                    GameObject arena = Instantiate(arenaPrefab, playerController.groups[i].GetUnits()[i].GetTransform().position, Quaternion.identity);

                    //position of battle, 2 groups in conflict, and arena asset 
                    playerController.battles.Add(new TurnBasedBattleController(new Vector3(playerController.groups[i].GetUnits()[i].GetTransform().position.x, 0.0f, playerController.groups[i].GetUnits()[i].GetTransform().position.z), playerController.groups[i], enemyController.enemyGroups[j], arena));

                    //i = 4;

                    //start battle for both units 
                    playerController.groups[i].BattleStarted();
                    enemyController.enemyGroups[j].BattleStarted();
                    break;
                }
            }
        }
    }
}
