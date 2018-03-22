using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO LIST:
//Get Selection GameObject
//Add a string array for names
//Work on guild hall
//Play with unit creation 
//
//

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

    public GuildHallController guildHall;

    void Awake()
    {
        guildHall = null;
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();

        List<DynamicUnit> myUnits;
        myUnits = new List<DynamicUnit>();
        myUnits.Add(CreatePlayerArcher(new Vector3(20, 0, 20)));
        myUnits.Add(CreatePlayerWarrior(new Vector3(20, 0, 20)));
        myUnits.Add(CreatePlayerMage(new Vector3(20, 0, 20)));

        // add some player units
        AddFactionGroup(myUnits, new Vector3(20, 0, 20), true);

        myUnits.Add(CreatePlayerArcher(new Vector3(80, 0, 30)));
        myUnits.Add(CreatePlayerWarrior(new Vector3(80, 0, 30)));
        myUnits.Add(CreatePlayerMage(new Vector3(80, 0, 30)));

        // add some player units
        AddFactionGroup(myUnits, new Vector3(80, 0, 30), true);

        myUnits.Add(CreatePlayerArcher(new Vector3(120, 0, 60)));
        myUnits.Add(CreatePlayerWarrior(new Vector3(120, 0, 60)));
        myUnits.Add(CreatePlayerMage(new Vector3(120, 0, 60)));

        // add some player units
        AddFactionGroup(myUnits, new Vector3(120, 0, 60), true);

        myUnits.Add(CreatePlayerArcher(new Vector3(160, 0, 40)));
        myUnits.Add(CreatePlayerWarrior(new Vector3(160, 0, 40)));
        myUnits.Add(CreatePlayerMage(new Vector3(160, 0, 40)));

        // add some player units
        AddFactionGroup(myUnits, new Vector3(160, 0, 40), true);

        myUnits.Add(CreatePlayerArcher(new Vector3(200, 0, 20)));
        myUnits.Add(CreatePlayerWarrior(new Vector3(200, 0, 20)));
        myUnits.Add(CreatePlayerMage(new Vector3(200, 0, 20)));

        // add some player units
        AddFactionGroup(myUnits, new Vector3(200, 0, 20), true);



    }

    void Update()
    {
        BattleCheck();
        GuildHallReturnCheck();
    }

    //Archer creation
    public FactionUnit CreatePlayerArcher(Vector3 position) { return Instantiate(roguePrefab, position, Quaternion.identity).GetComponent<RogueController>().unit; }
    //Warrior creation
    public FactionUnit CreatePlayerWarrior(Vector3 position) { return Instantiate(warriorPrefab, position, Quaternion.identity).GetComponent<WarriorController>().unit; }
    //mage creation
    public FactionUnit CreatePlayerMage(Vector3 position) { return Instantiate(magePrefab, position, Quaternion.identity).GetComponent<MageController>().unit; }
    //Enemy Archer creation
    public FactionUnit CreateEnemyArcher(Vector3 position) { return Instantiate(enemyArcherPrefab, position, Quaternion.identity).GetComponent<EnemyArcherController>().demonUnit; }
    //Enemy Infantry creation
    public FactionUnit CreateEnemyWarrior(Vector3 position) { return Instantiate(enemyInfantryPrefab, position, Quaternion.identity).GetComponent<EnemyInfantryController>().demonUnit; }
    //Enemy mage creation
    public FactionUnit CreateEnemyMage(Vector3 position) { return Instantiate(enemyMagePrefab, position, Quaternion.identity).GetComponent<EnemyMageController>().demonUnit; }

    //Adding to a faction's grps
    public void AddFactionGroup(List<DynamicUnit> factionUnits, Vector3 position, bool isPlayer)
    {
        Group grp = Instantiate(groupPrefab, position, Quaternion.identity).GetComponent<Group>();

        for (int i = 0; i < factionUnits.Count; i++)
        {
            grp.AddUnit(factionUnits[i]);
        }

        // Add either to player or enemy
        if (isPlayer) { playerController.groups.Add(grp); }
        else { enemyController.enemyGroups.Add(grp); }

        factionUnits.Clear();
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

                for (int k = 0; k < playerController.groups[i].GetUnits().Count; k++)
                {
                    for (int l = 0; l < enemyController.enemyGroups[j].GetUnits().Count; l++)
                    {
                        if (Vector3.Distance(playerController.groups[i].GetUnits()[k].GetTransform().position, enemyController.enemyGroups[j].GetUnits()[l].GetTransform().position) < 4)
                        {
                            GameObject arena = Instantiate(arenaPrefab, playerController.groups[i].GetUnits()[k].GetTransform().position, Quaternion.identity);

                            //position of battle, 2 groups in conflict, and arena asset 
                            playerController.battles.Add(new TurnBasedBattleController(new Vector3(playerController.groups[i].GetUnits()[k].GetTransform().position.x, 0.0f, playerController.groups[i].GetUnits()[k].GetTransform().position.z), playerController.groups[i], enemyController.enemyGroups[j], arena));

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
    }


    void GuildHallReturnCheck()
    {
        if (guildHall != null)
        {
            for (int i = 0; i < playerController.groups.Count; i++)
            {
                //Change from destination to the actual player's mouse click
                if (guildHall.gameObject.GetComponent<BoxCollider>().bounds.Contains(playerController.groups[i].rawDestination))
                {
                    playerController.groups[i].returningToGuildHall = true;
                    playerController.groups[i].DisableCollisionsWithCollider(guildHall.gameObject.GetComponent<BoxCollider>());
                    playerController.groups[i].ResetGroupDestination();
                    for (int j = 0; j < playerController.groups[i].GetUnits().Count; j++)
                    {
                        if (guildHall.gameObject.GetComponent<BoxCollider>().bounds.SqrDistance(playerController.groups[i].GetUnits()[j].GetTransform().position) < 6)
                        {
                            playerController.groups[i].RemoveBumperCars();
                            playerController.groups[i].ResetGroupDestination();

                            if (guildHall.gameObject.GetComponent<BoxCollider>().bounds.SqrDistance(playerController.groups[i].GetUnits()[j].GetTransform().position) < 3)
                            {
                                playerController.groups[i].GetUnits()[j].GetAgent().areaMask = 5;
                                guildHall.ReturnUnit(playerController.groups[i], j);
                                if (playerController.groups[i].GetUnits().Count == 0)
                                {
                                    playerController.groups.RemoveAt(i);
                                    i--;
                                }
                                else
                                {
                                    playerController.groups[i].ResetGroupDestination();
                                }
                            }
                        }
                    }
                }
                else if (!playerController.groups[i].returningToGuildHall)
                {
                    playerController.groups[i].ReInitializeBumperCars();
                    playerController.groups[i].EnableCollisionsWithCollider(guildHall.gameObject.GetComponent<BoxCollider>());
                    playerController.groups[i].returningToGuildHall = false;
                }
            }
        }
    }


    public bool DestinationWithinTarget(Vector3 destination, GameObject target)
    {
        return target.GetComponent<BoxCollider>().bounds.Contains(destination);
    } 

   

}
