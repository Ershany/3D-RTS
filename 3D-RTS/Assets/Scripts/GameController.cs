﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TO DO LIST:
//Add a string array for names
//Buildings:
//logic
//UI
//static units
//fix battles completely 

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

    //Static Untis
    public GameObject trollPrefab;
    public GameObject goblinPrefab;
    public GameObject wolfPrefab;
    //public GameObject spiderPrefab; //maybe not the spider its not done yet 

    // UI stuff
    public GameObject groupPrefab;
    public GameObject arenaPrefab;

    // buildings
    public GameObject blacksmithPrefab;
    public GameObject ArcheryRangePrefab;
    public GameObject TempleOfMagiPrefab;
    public GameObject GuildHallPrefab;

    /* References */
    private PlayerController playerController;
    private EnemyController enemyController;
    public GuildHallController guildHall;

    private List<RandomEncounterController> randomEncounters;
    //Dictionary
    public string[] unitNames;

    //player and enemy resources
    public int enemyGold;

    public bool gameOver;
    public bool playerWon;
    private Text gameOverText;

    void Awake()
    {
        //Get all the random encounters to check for their battles 
        randomEncounters = new List<RandomEncounterController>();
        GameObject[] encounters = GameObject.FindGameObjectsWithTag("RandomEncounter");

        for (int i = 0; i < encounters.Length; i++)
        {
            randomEncounters.Add(encounters[i].GetComponent<RandomEncounterController>());
        }

        //setting up static units prefabs for random encounters
        EnvironmentUnits.neutralUnits = new List<GameObject>();
        //EnvironmentUnits.staticUnits.Add(trollPrefab);
        EnvironmentUnits.neutralUnits.Add(wolfPrefab);
        //EnvironmentUnits.staticUnits.Add(spiderPrefab);
        //EnvironmentUnits.staticUnits.Add(goblinPrefab);

        //gold
        enemyGold = 200;

        guildHall = null;
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();

        //SOME RANDOM NAMES JUST FOR THE FUN OF IT 
        unitNames = new string[] { "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , ""};

        
        /* Create some player units */
        //List<DynamicUnit> myUnits;
        //myUnits = new List<DynamicUnit>();
        //myUnits.Add(CreatePlayerArcher(new Vector3(20, 0, 20)));
        //myUnits.Add(CreatePlayerWarrior(new Vector3(20, 0, 20)));
        //myUnits.Add(CreatePlayerMage(new Vector3(20, 0, 20)));

        //// add some player units
        //AddFactionGroup(myUnits, new Vector3(20, 0, 20), true);

        //myUnits.Add(CreatePlayerArcher(new Vector3(80, 0, 30)));
        //myUnits.Add(CreatePlayerWarrior(new Vector3(80, 0, 30)));
        //myUnits.Add(CreatePlayerMage(new Vector3(80, 0, 30)));

        //// add some player units
        //AddFactionGroup(myUnits, new Vector3(80, 0, 30), true);



        /* Create some enemy units */
        Vector3 enemyPos = new Vector3(412.0f, 0.0f, 450.0f);
        EnemyArcherController archer = Instantiate(enemyArcherPrefab, enemyPos, Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry = Instantiate(enemyInfantryPrefab, enemyPos, Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage = Instantiate(enemyMagePrefab, enemyPos, Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        Vector3 enemyPos1 = new Vector3(375.0f, 0.0f, 445.0f);
        EnemyArcherController archer1 = Instantiate(enemyArcherPrefab, enemyPos1, Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry1 = Instantiate(enemyInfantryPrefab, enemyPos1 + new Vector3(4.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage1 = Instantiate(enemyMagePrefab, enemyPos1 + new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        Vector3 enemyPos2 = new Vector3(400.0f, 0.0f, 445.0f);
        EnemyArcherController archer2 = Instantiate(enemyArcherPrefab, enemyPos2, Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry2 = Instantiate(enemyInfantryPrefab, enemyPos2 + new Vector3(4.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage2 = Instantiate(enemyMagePrefab, enemyPos2 + new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        Vector3 enemyPos3 = new Vector3(410.0f, 0.0f, 430.0f);
        EnemyArcherController archer3 = Instantiate(enemyArcherPrefab, enemyPos3, Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry3 = Instantiate(enemyInfantryPrefab, enemyPos3 + new Vector3(4.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage3 = Instantiate(enemyMagePrefab, enemyPos3 + new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        List<DynamicUnit> enemyUnits = new List<DynamicUnit>();
        enemyUnits.Add(archer.demonUnit);
        enemyUnits.Add(infantry.demonUnit);
        enemyUnits.Add(mage.demonUnit);
        AddFactionGroup(enemyUnits, enemyPos, false);

        List<DynamicUnit> enemyUnits1 = new List<DynamicUnit>();
        enemyUnits1.Add(archer1.demonUnit);
        enemyUnits1.Add(infantry1.demonUnit);
        enemyUnits1.Add(mage1.demonUnit);
        AddFactionGroup(enemyUnits1, enemyPos1, false);

        List<DynamicUnit> enemyUnits2 = new List<DynamicUnit>();
        enemyUnits2.Add(archer2.demonUnit);
        enemyUnits2.Add(infantry2.demonUnit);
        enemyUnits2.Add(mage2.demonUnit);
        AddFactionGroup(enemyUnits2, enemyPos2, false);

        List<DynamicUnit> enemyUnits3 = new List<DynamicUnit>();
        enemyUnits3.Add(archer3.demonUnit);
        enemyUnits3.Add(infantry3.demonUnit);
        enemyUnits3.Add(mage3.demonUnit);
        AddFactionGroup(enemyUnits3, enemyPos3, false);


        gameOver = false;
        playerWon = false;
        gameOverText = GameObject.FindGameObjectWithTag("EndGameText").GetComponent<Text>();
    }

    void Update()
    {
        //check for battles between units
        BattleCheck();

        //check for units returning back to guild hall
        GuildHallReturnCheck();

        // Check for end state
        if (gameOver)
        {
            if (playerWon) gameOverText.text = "You Win!";
            else gameOverText.text = "You Lose!";
        }
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

    //I think this is a good place to add these or maybe in a util file

    public Building CreateGuildHall(bool isPlayer)
    {
        //maybe??
        return null;
    }

    //check if player's or enemy's
    public Building CreateBlacksmith(bool isPlayer)
    {
        //instantiate blacksmith 
        TechnologyBuildingController controller = Instantiate(blacksmithPrefab, Vector3.zero, Quaternion.Euler(0 , -225 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> blackSmithTechnologies = new List<string> { "Reinforced Armor" , "Courage" , "Spell Mastery" , "Education" , "Sword Mastery"};
        List<int> blacksmithBuffs = new List<int> { 1 , 1 , 0 , 0 , 0};
        string unitAffected = "Everyone"; //might need to be changed
        float health = 200.0f;

        controller.SetupBuilding(blackSmithTechnologies , blacksmithBuffs , unitAffected , health , "Blacksmith" , isPlayer);
        Building building = controller.building;
        
        //add reference to player or enemy whichever one owns the building
        if (isPlayer) { playerController.playerBuildings.Add(building); }
        else { enemyController.enemyBuildings.Add(building); }

        return building;
    }

    //check if player's or enemy's
    public Building CreateArcheryRange(bool isPlayer)
    {
        TechnologyBuildingController controller = Instantiate(ArcheryRangePrefab, Vector3.zero, Quaternion.Euler(-90 , 225 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> ArcheryRangeTechnologies = new List<string> { "Padding", "Resolve", "Spell Mastery", "Mental Strength", "Perception" };
        List<int> ArcheryRangeBuffs = new List<int> { 0, 0, 0, 0, 1 };
        string unitAffected = "Everyone"; //might need to be changed
        float health = 150.0f;

        controller.SetupBuilding(ArcheryRangeTechnologies, ArcheryRangeBuffs, unitAffected, health , "ArcheryRange" , isPlayer);
        Building building = controller.building;
        //add reference to player or enemy whichever one owns the building

        if (isPlayer) { playerController.playerBuildings.Add(building); }
        else { enemyController.enemyBuildings.Add(building); }

        return building;
    }

    //check if player's or enemy's
    public Building CreateTempleOfMagi(bool isPlayer)
    {
        TechnologyBuildingController controller = Instantiate(TempleOfMagiPrefab, Vector3.zero, Quaternion.Euler(0 , 135 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> templeOfMagiTechnologies = new List<string> { "Armored Robes", "Mutations", "Mage Training", "Wisdom", "Contemplation" };
        List<int> templeOfmagiBuffs = new List<int> { 0, 0, 1, 1, 0 };
        string unitAffected = "Everyone"; //might need to be changed
        float health = 300.0f;

        controller.SetupBuilding(templeOfMagiTechnologies, templeOfmagiBuffs, unitAffected, health , "TempleOfMagi" , isPlayer);
        Building building = controller.building;

        //add reference to player or enemy whichever one owns the building
        if (isPlayer) { playerController.playerBuildings.Add(building); }
        else { enemyController.enemyBuildings.Add(building); }

        return building;
    }

    //Adding to a faction's grps
    public void AddFactionGroup(List<DynamicUnit> factionUnits, Vector3 position, bool isPlayer)
    {
        Group grp = Instantiate(groupPrefab, position, Quaternion.identity).GetComponent<Group>();

        if (factionUnits.Count > 0)
        {
            grp.AddUnit(factionUnits[0]);

            for (int i = 1; i < factionUnits.Count; i++)
            {
                grp.AddUnit(factionUnits[i]);
            }
        }
        

        // Add either to player or enemy
        if (isPlayer) { playerController.groups.Add(grp); }
        else { enemyController.AddGroup(grp); }

        factionUnits.Clear();
    }

    public Group CreateGroup(List<DynamicUnit> units, Vector3 position)
    {
        Group grp = Instantiate(groupPrefab, position, Quaternion.identity).GetComponent<Group>();

        for (int i = 0; i < units.Count; i++)
        {
            if (i != 0)
            {
                units[i].GetTransform().position = units[i - 1].GetTransform().position + new Vector3(4.0f, 0, 0);
            }
            else
            {
                units[0].GetTransform().position += new Vector3(4.0f, 0, -0.0f);
            }

            grp.AddUnit(units[i]);
        }

        return grp;

    }


    //Check for a battle instance 
    void BattleCheck()
    {
        //check for battles with player's units
        for (int i = 0; i < playerController.groups.Count; i++)
        {
            //if group is in battle no need to check
            if (playerController.groups[i].GetFirstUnit().IsInBattle) continue;

            //check for static units 
            
            for (int w = 0; w < randomEncounters.Count; w++)
            {
                randomEncounters[w].UpdateRandomBattleController();
                //if we don't have a created unit or the unit is not in battle
                if (randomEncounters[w].neutralUnits == null) continue;

                if (randomEncounters[w].battleEncountered)
                {
                    //if (Vector3.SqrMagnitude(randomEncounters[w].transform.position - playerController.groups[i].GetFirstUnit().GetTransform().position) < 25)


                    //battle with static unit
                    if (randomEncounters[w].GetUnits(0)[0].GetGameObject() == null)
                        continue;
                    
                    GameObject arena = Instantiate(arenaPrefab, randomEncounters[w].battleGroups[0].GetFirstUnit().GetTransform().position, Quaternion.identity);

                    //position of battle, 2 groups in conflict, and arena asset 
                    playerController.battles.Add(new TurnBasedBattleController(new Vector3(
                        randomEncounters[w].battleGroups[0].GetFirstUnit().GetTransform().position.x,
                        0.0f, randomEncounters[w].battleGroups[0].GetFirstUnit().GetTransform().position.z),
                        randomEncounters[w].battleGroups[0], randomEncounters[w].GetUnits(0), arena));

                    //start battle for both units 
                    randomEncounters[w].neutralUnits.RemoveAt(0);
                    randomEncounters[w].battleGroups.RemoveAt(0);
                    randomEncounters[w].battleEncountered = false;
                    playerController.groups[i].BattleStarted();
                }
            }

            //check enemy Units
            for (int j = 0; j < enemyController.enemyGroups.Count; j++)
            {
                //if enemy is in battle continue
                if (enemyController.enemyGroups[j].GetFirstUnit().IsInBattle) continue;

                for (int k = 0; k < playerController.groups[i].GetUnits().Count; k++)
                {
                    for (int l = 0; l < enemyController.enemyGroups[j].GetUnits().Count; l++)
                    {
                        if (Vector3.SqrMagnitude(playerController.groups[i].GetUnits()[k].GetTransform().position - enemyController.enemyGroups[j].GetUnits()[l].GetTransform().position) < 25)
                        {
                            GameObject arena = Instantiate(arenaPrefab, playerController.groups[i].GetUnits()[k].GetTransform().position, Quaternion.identity);

                            //position of battle, 2 groups in conflict, and arena asset 
                            playerController.battles.Add(new TurnBasedBattleController(new Vector3(playerController.groups[i].GetUnits()[k].GetTransform().position.x, 0.0f, playerController.groups[i].GetUnits()[k].GetTransform().position.z), playerController.groups[i], enemyController.enemyGroups[j], arena));
                            
                            //start battle for both units 
                            playerController.groups[i].BattleStarted();
                            enemyController.enemyGroups[j].BattleStarted();
                            break;
                        }
                    }
                }
            }
        }

        BuildingBattleCheck();
    }

    //Currently used for checking battles with the player's GuildHall and enemyGroups.
    //Plan on expanding code to include enemy buildings when they become available.
    void BuildingBattleCheck()
    {
        if (!playerController.guildHallBuilt)
            return;
        for (int i = 0; i < enemyController.enemyGroups.Count; i++)
        {
            if (enemyController.enemyGroups[i].GetFirstUnit().IsInBattle)
                continue;
            for (int j = 0; j < enemyController.enemyGroups[i].GetUnits().Count; j++) {
                if (Vector3.SqrMagnitude(guildHall.transform.position - enemyController.enemyGroups[i].GetUnits()[j].GetTransform().position) < 400)
                {
                    GameObject arena = Instantiate(arenaPrefab, enemyController.enemyGroups[i].GetUnits()[j].GetTransform().position, Quaternion.identity);

                    guildHall.InitiateBattle(enemyController.enemyGroups[i], enemyController.enemyGroups[i].GetUnits()[j].GetTransform().position, arena);

                    enemyController.enemyGroups[i].BattleStarted();
                    break;
                }
            }
        }
    }
    //checking for units that want to return to the guild hall
    void GuildHallReturnCheck()
    {
        // if we don't have groups or a guild hall return
        if (playerController.groups.Count < 1 || !guildHall) return;

        // we have a guild hall and we have groups in this case 
        if (guildHall)
        {
            //get collider
            BoxCollider guildHallCollider = guildHall.gameObject.GetComponent<BoxCollider>();

            //check for a returning unit
            for (int i = 0; i < playerController.groups.Count; i++)
            {
                // if i clicked at the guild hall return this unit to the guild hall
                if (guildHallCollider.bounds.Contains(playerController.groups[i].rawDestination))
                {
                    // return group to guild hall 
                    playerController.groups[i].returningToGuildHall = true;
                    playerController.groups[i].DisableCollisionsWithCollider(guildHallCollider);
                    playerController.groups[i].ResetGroupDestination();
                }

                //check for returing units if they are close enough to be added back to the guild hall 
                for (int j = 0; j < playerController.groups[i].GetUnits().Count; j++)
                {
                    // if they are not returning to guild hall ignore this check
                    if (!playerController.groups[i].returningToGuildHall) return;

                    //check distance squared to the guild hall to allow returning back
                    if (guildHallCollider.bounds.SqrDistance(playerController.groups[i].GetUnits()[j].GetTransform().position) < 6)
                    {
                        //deselect group first (i set the function to be public in playerController)
                        playerController.HighlightGroup(playerController.groups[i] , false);

                        //add the entire group back to guild hall
                        for (int w = 0; w < playerController.groups[i].GetUnits().Count; w++)
                        {
                            guildHall.roster.Add(playerController.groups[i].GetUnits()[w]);
                            playerController.groups[i].GetUnits()[w].GetGameObject().SetActive(false);
                            playerController.groups[i].GetUnits()[w].SetGameObjectParent(null);
                        }

                        //clear the group's unit , delete the group gameObject and remove the group itself
                        playerController.groups[i].GetUnits().Clear();
                        Destroy(playerController.groups[i].gameObject);
                        playerController.groups.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }

        //julian's code here
        /*
        if (guildHall != null && playerController.groups.Count > 0)
        {
            for (int i = 0; i < playerController.groups.Count; i++)
            {
                if(playerController.groups[i].GetUnits().Count < 1)
                {
                    playerController.groups.RemoveAt(i);
                    i--;
                    continue;
                }

          
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
                                //playerController.groups[i].GetUnits()[j].GetAgent().areaMask = 5;
                                guildHall.ReturnUnit(playerController.groups[i], j);
                                j = 0;
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
        */
    }


    public bool DestinationWithinTarget(Vector3 destination, GameObject target)
    {
        return target.GetComponent<BoxCollider>().bounds.Contains(destination);
    } 


}
