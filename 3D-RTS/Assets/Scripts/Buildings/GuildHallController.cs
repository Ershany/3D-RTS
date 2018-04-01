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

    void Awake()
    {
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
        selectedRosterUnitNum = -1;

    }

    void Update()
    {
        building.Update();
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
        if (selectedNewUnitNum == 1)
        {
            FactionUnit tempUnit = gameController.CreatePlayerWarrior(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
        }
        else if (selectedNewUnitNum == 2)
        {
            FactionUnit tempUnit = gameController.CreatePlayerArcher(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
        }
        else if (selectedNewUnitNum == 3)
        {
            FactionUnit tempUnit = gameController.CreatePlayerMage(new Vector3());
            tempUnit.GetGameObject().SetActive(false);
            roster.Add(tempUnit);
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
            // reset gameObjects to not be active

            roster.Add(myUnits.GetUnits()[i]);
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
