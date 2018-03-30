using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO LIST:
//Add a string array for names
//Buildings:
//logic
//UI

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

    //Dictionary
    public string[] unitNames;

    //player and enemy resources
    public int playerGold;
    public int enemyGold;

    void Awake()
    {
        //gold
        playerGold = 200;
        enemyGold = 200;

        guildHall = null;
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();

        //SOME RANDOM NAMES JUST FOR THE FUN OF IT 
        unitNames = new string[] { "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , "" , ""};

        /* Create some player units */
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
        TechnologyBuildingController controller = Instantiate(ArcheryRangePrefab, Vector3.zero, Quaternion.Euler(0 , 90 , 0)).GetComponent<TechnologyBuildingController>();

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

    //checking for units that want to return to the guild hall
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

    // ????
    public bool DestinationWithinTarget(Vector3 destination, GameObject target)
    {
        return target.GetComponent<BoxCollider>().bounds.Contains(destination);
    } 
}
