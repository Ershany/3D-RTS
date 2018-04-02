using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int playerGold;
    public int enemyGold;

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
        EnvironmentUnits.staticUnits = new List<GameObject>();
        EnvironmentUnits.staticUnits.Add(trollPrefab);
        EnvironmentUnits.staticUnits.Add(wolfPrefab);
        //EnvironmentUnits.staticUnits.Add(spiderPrefab);
        EnvironmentUnits.staticUnits.Add(goblinPrefab);

        //gold
        playerGold = 200;
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
        EnemyArcherController archer = Instantiate(enemyArcherPrefab, new Vector3(5.0f, 0.0f, 40.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyArcherController>();
        EnemyInfantryController infantry = Instantiate(enemyInfantryPrefab, new Vector3(5.0f, 0.0f, 40.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyInfantryController>();
        EnemyMageController mage = Instantiate(enemyMagePrefab, new Vector3(5.0f, 0.0f, 40.0f), Quaternion.identity, gameObject.transform).GetComponent<EnemyMageController>();

        List<DynamicUnit> enemyUnits = new List<DynamicUnit>();
        enemyUnits.Add(archer.demonUnit);
        enemyUnits.Add(infantry.demonUnit);
        enemyUnits.Add(mage.demonUnit);

        //Debug.Log("enemy groups: " + enemyController.enemyGroups.Count); 
        AddFactionGroup(enemyUnits, new Vector3(5.0f, 0.0f, 40.0f), false);
    }

    void Update()
    {
        //check for battles between units
        BattleCheck();

        //check for units returning back to guild hall
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
        TechnologyBuildingController controller = Instantiate(blacksmithPrefab, Vector3.zero, Quaternion.Euler(0 , -180 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> blackSmithTechnologies = new List<string> { "Reinforced Armor" , "Courage" , "Spell Mastery" , "Education" , "Sword Mastery"};
        List<int> blacksmithBuffs = new List<int> { 3 , 1 , 1 , 2 , 2};
        string unitAffected = "Warrior"; //might need to be changed
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
        TechnologyBuildingController controller = Instantiate(ArcheryRangePrefab, Vector3.zero, Quaternion.Euler(-90 , 180 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> ArcheryRangeTechnologies = new List<string> { "Padding", "Resolve", "Spell Mastery", "Mental Strength", "Perception" };
        List<int> ArcheryRangeBuffs = new List<int> { 2, 2, 1, 1, 3 };
        string unitAffected = "Archer"; //might need to be changed
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
        TechnologyBuildingController controller = Instantiate(TempleOfMagiPrefab, Vector3.zero, Quaternion.Euler(0 , 180 , 0)).GetComponent<TechnologyBuildingController>();

        List<string> templeOfMagiTechnologies = new List<string> { "Armored Robes", "Mutations", "Mage Training", "Wisdom", "Contemplation" };
        List<int> templeOfmagiBuffs = new List<int> { 0, 0, 0, 0, 0 };
        string unitAffected = "Mage"; //might need to be changed
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

        for (int i = 0; i < factionUnits.Count; i++)
        {
            if (i != 0)
            {
                factionUnits[i].GetTransform().position = factionUnits[i - 1].GetTransform().position + new Vector3(4.0f, 0, 0);
            }
            else
            {
                factionUnits[0].GetTransform().position += new Vector3(4.0f, 0, -0.0f);
            }

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
        //check for battles with player's units
        for (int i = 0; i < playerController.groups.Count; i++)
        {
            //if group is in battle no need to check
            if (playerController.groups[i].GetFirstUnit().IsInBattle) continue;

            //check for static units 
            for (int w = 0; w < randomEncounters.Count; w++)
            {
                //if we don't have a created unit or the unit is not in battle
                if (randomEncounters[w].staticUnit == null || randomEncounters[w].staticUnit.unit.IsInBattle) continue;

                if (Vector3.SqrMagnitude(randomEncounters[w].staticUnit.transform.position - playerController.groups[i].GetFirstUnit().GetTransform().position) < 25)
                {
                    //battle with static unit
                    //GameObject arena = Instantiate(arenaPrefab , playerController.groups[i].GetFirstUnit().GetTransform().position , Quaternion.identity);

                    //position of battle, 2 groups in conflict, and arena asset 
                    //playerController.battles.Add(new TurnBasedBattleController(new Vector3(playerController.groups[i].GetUnits()[k].GetTransform().position.x, 0.0f, playerController.groups[i].GetUnits()[k].GetTransform().position.z), playerController.groups[i], enemyController.enemyGroups[j], arena));

                    //start battle for both units 
                    //playerController.groups[i].BattleStarted();
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

    // ????
    public bool DestinationWithinTarget(Vector3 destination, GameObject target)
    {
        return target.GetComponent<BoxCollider>().bounds.Contains(destination);
    } 
}
