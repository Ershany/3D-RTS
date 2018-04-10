using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHallController : MonoBehaviour
{
    // Unit Stats
    [Range(1.0f, 1000.0f)] public float buildingHealth = 500.0f;

    // References
    public Building building;
    private GameController gameController;
    private PlayerController playerController;

    // Units
    public List<DynamicUnit> roster;
    public List<DynamicUnit> unitsToBeDeployed;

    public List<DynamicUnit> defaultUnits;

    public int selectedNewUnitNum;
    public int selectedRosterUnitNum;

    public int[] goldCosts;

    public GameObject battleRepPrefab;
    private List<DynamicUnit> battleRepresentatives;
    public List<TurnBasedBattleController> guildHallBattles;
    public List<Group> battleGroups;

    void Awake()
    {
        goldCosts = new int[3];
        goldCosts[0] = 10;
        goldCosts[1] = 10;
        goldCosts[2] = 10;
        //setup members 
        //for now
        building = new Building(gameObject, Building.BuildingType.GUILDHALL, buildingHealth , "GuildHall" , true);


        building.name = "GuildHall";
        roster = new List<DynamicUnit>();
        unitsToBeDeployed = new List<DynamicUnit>();


        //Reference game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        defaultUnits = new List<DynamicUnit>();
        defaultUnits.Add(new FactionUnit("Warrior"));
        defaultUnits.Add(new FactionUnit("Archer"));
        defaultUnits.Add(new FactionUnit("Mage"));


        selectedNewUnitNum = -1;
        selectedRosterUnitNum = 0;

        guildHallBattles = new List<TurnBasedBattleController>();
        battleRepresentatives = new List<DynamicUnit>();
        battleGroups = new List<Group>();

    }

    void Update()
    {
        building.Update();
        BattleUpdate();
    }

    //Add a unit to the roster
    public void CreateUnit(int unitNum)
    { 
        //will have to check which type of unit we are attempting to create
        //setActive of game Object to false first

        //NEEDS CHANGESSSSS!!!!
        if (unitNum == 1)
        {
            FactionUnit tempUnit = gameController.CreatePlayerWarrior(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
        }
        else if (unitNum == 2)
        {
            FactionUnit tempUnit = gameController.CreatePlayerArcher(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
        }
        else if (unitNum == 3)
        {
            FactionUnit tempUnit = gameController.CreatePlayerMage(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
        }
    }

    public void CreateUnit()
    {
        //will have to check which type of unit we are attempting to create
        //setActive of game Object to false first

        //NEEDS CHANGING
        if (playerController.playerGold > 10)
        {
            if (selectedNewUnitNum == 1)
            {
                FactionUnit tempUnit = gameController.CreatePlayerWarrior(new Vector3());
                tempUnit.GetGameObject().SetActive(false);
                roster.Add(tempUnit);
                playerController.playerGold -= 10;
            }
            else if (selectedNewUnitNum == 2)
            {
                FactionUnit tempUnit = gameController.CreatePlayerArcher(new Vector3());
                tempUnit.GetGameObject().SetActive(false);
                roster.Add(tempUnit);
                playerController.playerGold -= 10;
            }
            else if (selectedNewUnitNum == 3)
            {
                FactionUnit tempUnit = gameController.CreatePlayerMage(new Vector3());
                tempUnit.GetGameObject().SetActive(false);
                roster.Add(tempUnit);
                playerController.playerGold -= 10;
            }
        }
    }

    public void SetSelectedUnitNum(int i)
    {
        Debug.Log("GUILDCON SELECTEDUNITNUM = " + i);
        selectedNewUnitNum = i;
    }

    //add units to be deployed
    public void AddToDeployedUnits(int unitIndex)
    {
        //order buttons and roster array parallely

        //remove unit from roster and add it to the units to be deployed
        unitsToBeDeployed.Add(roster[unitIndex]);
        roster.RemoveAt(unitIndex);

        //IF WE WANT TO RETURN UNITS BACK CUZ WE CHANGED OUR MIND WE CAN JUST CALL RETURNUNITS
        //make UI changes here (we could repopulate the roster ui buttons)

        Debug.Log("added unit to temporary deploy roster");
    }



    public void RemoveFromDeployedUnits(int unitIndex)
    {
        //order buttons and roster array parallely

        //remove unit from roster and add it to the units to be deployed
        roster.Add(unitsToBeDeployed[unitIndex]);
        unitsToBeDeployed.RemoveAt(unitIndex);

        //IF WE WANT TO RETURN UNITS BACK CUZ WE CHANGED OUR MIND WE CAN JUST CALL RETURNUNITS
        //make UI changes here (we could repopulate the roster ui buttons)

        Debug.Log("added unit to temporary deploy roster");
    }

    //deploy units
    public void DeployUnits()
    {
        //make a group of the units
        //reset their gameObject's active to true
        if (unitsToBeDeployed.Count > 0)
        {

            for(int i = 0; i < unitsToBeDeployed.Count; i++)
            {
                Debug.Log((Vector3.Normalize(Vector3.Cross(transform.up, transform.forward))));
                unitsToBeDeployed[i].GetTransform().position = transform.position + transform.up * 12 + (i * 6 * (Vector3.Normalize(Vector3.Cross(transform.up, transform.forward))));
                unitsToBeDeployed[i].GetGameObject().SetActive(true);
            }
            Debug.Log(transform.position);
            gameController.AddFactionGroup(unitsToBeDeployed, unitsToBeDeployed[0].GetTransform().position, true);
            unitsToBeDeployed.Clear();

            Debug.Log("Deployed group");
        }
    }

    //return units back to roster
    public void ReturnGroup(Group myUnits)
    {
        for (int i = 0; i < myUnits.GetUnits().Count; i++)
        {
            if (myUnits.GetUnits()[i].GetClassName() != "GuildHall")
            {
                roster.Add(myUnits.GetUnits()[i]);
            }
            else
            {
                battleRepresentatives.Remove(myUnits.GetUnits()[i]);
                Destroy(myUnits.GetUnits()[i].GetGameObject());
            }
            myUnits.GetUnits().RemoveAt(i);
                i--;
        }

        //delete group 

        //myUnits.GetUnits().Clear();
        //DO UI stuff here

        Debug.Log("Returned units back to roster");
    }

    public void ReturnUnit(Group myGroup, int i)
    {
        roster.Add(myGroup.GetUnits()[i]);
        myGroup.GetUnits()[i].GetGameObject().SetActive(false);
        myGroup.GetUnits().RemoveAt(i);
        

    }

    public void InitiateBattle(Group enemyGroup, Vector3 initiatingUnitPosition, GameObject arenaObject)
    {
        Group gGroup = gameController.CreateGroup(new List<DynamicUnit>(), initiatingUnitPosition);

        gGroup.AddUnit(CreateBattleRepresentative());

        for (int i = 0; i < 3; i++)
        {
            if (roster.Count < 1)
                continue;
            int index = Random.Range(0, roster.Count);
            gGroup.AddUnit(roster[index]);
            roster[index].GetGameObject().SetActive(true);
            roster.RemoveAt(index);
        }


        battleGroups.Add(gGroup);

        TurnBasedBattleController tbbc = new TurnBasedBattleController(initiatingUnitPosition, gGroup, enemyGroup, arenaObject);

        guildHallBattles.Add(tbbc);
    }

    private void BattleUpdate()
    {

        for (int i = 0; i < guildHallBattles.Count; i++)
        {
            guildHallBattles[i].Update();

            while (guildHallBattles[i].playerGroup.GetUnits().Count < 4 && roster.Count > 1)
            {

                int index = Random.Range(0, roster.Count);
                guildHallBattles[i].AddUnitToGroupInBattle(roster[index], guildHallBattles[i].playerGroup);
                roster[index].GetGameObject().SetActive(true);
                roster.RemoveAt(index);
            }

            if (guildHallBattles[i].battleOver)
            {
                bool playersWon = true;
                int enemyCount = 0;


                guildHallBattles[i].enemyGroup.BattledEnded();

                for (int j = 0; j < guildHallBattles[i].enemyGroup.GetUnits().Count; j++)
                {
                    enemyCount += 5;
                    if (!guildHallBattles[i].enemyGroup.GetUnits()[j].IsDead)
                        playersWon = false;
                    Destroy(guildHallBattles[i].enemyGroup.GetUnits()[j].GetGameObject());
                }

                guildHallBattles[i].playerGroup.BattledEnded();

                if (!guildHallBattles[i].playerGroup.GetUnits()[0].IsPlayerControlled)
                    continue;

                int groupIndex = -1;
                for (int j = 0; j < battleGroups.Count; j++)
                {
                    if (guildHallBattles[i].playerGroup == battleGroups[j])
                        groupIndex = j;
                }
                if (groupIndex == -1)
                    continue;
                for (int j = 0; j < battleGroups[groupIndex].GetUnits().Count; j++)
                {
                    if (battleGroups[groupIndex].GetUnits()[j].IsDead)
                    {
                        DynamicUnit temp = battleGroups[groupIndex].GetUnits()[j];
                        battleGroups[groupIndex].GetUnits().RemoveAt(j);
                        Destroy(temp.GetGameObject());

                    }
                    if (playersWon)
                    {
                        playerController.playerGold += enemyCount;
                        battleGroups[groupIndex].GetUnits()[j].GiveExp(enemyCount * 15);
                        //Give players rewards

                    }
                }
                guildHallBattles[i].playerGroup.BattledEnded();
                Destroy(guildHallBattles[i].arena);
                guildHallBattles.RemoveAt(i);
                ReturnGroup(battleGroups[groupIndex]);
                i--;
            }
        }

        for (int i = 0; i < battleRepresentatives.Count; i++)
        {
            buildingHealth = Mathf.Min(buildingHealth, battleRepresentatives[i].GetCurrentHealth());
        }

        if (buildingHealth < 0)
        {
            gameController.gameOver = true;
        }

    }

    DynamicUnit CreateBattleRepresentative()
    {
        GameObject representative = Instantiate(battleRepPrefab);
        FactionUnit battleRepresentative = new FactionUnit(representative, true, "GuildHall");

        battleRepresentative.TakeDamage(battleRepresentative.GetCurrentHealth() - buildingHealth);  

        battleRepresentatives.Add(battleRepresentative);
        return battleRepresentative;
    }


    void OnTriggerEnter(Collider other)
    {
        building.OnTriggerEnter();
    }

    void OnTriggerExit(Collider other)
    {
        building.OnTriggerExit();
    }

    public void GuildHallPlaced()
    {
        gameController.guildHall = this;
    }
}
