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

    public int selectedUnitNum;


    void Awake()
    {
        //setup members 
        building = new Building(gameObject, buildingHealth);
        building.name = "GuildHall";
        roster = new List<DynamicUnit>();
        unitsToBeDeployed = new List<DynamicUnit>();


        //Reference game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        defaultUnits = new List<DynamicUnit>();
        defaultUnits.Add(gameController.CreatePlayerWarrior());
        defaultUnits.Add(gameController.CreatePlayerArcher());
        defaultUnits.Add(gameController.CreatePlayerMage());

        for (int i = 0; i < defaultUnits.Count; i++)
        {
            defaultUnits[i].GetTransform().position = new Vector3(-30 + i * 6, -30, -30);
        }

        selectedUnitNum = -1;

    }

    //Add a unit to the roster
    public void CreateUnit(int unitNum)
    { 
        //will have to check which type of unit we are attempting to create
        //setActive of game Object to false first

        if (unitNum == 1)
        {
            roster.Add(gameController.CreatePlayerWarrior());
        }
        else if (unitNum == 2)
        {
            roster.Add(gameController.CreatePlayerArcher());
        }
        else if (unitNum == 3)
        {
            roster.Add(gameController.CreatePlayerMage());
        }
    }

    public void CreateUnit()
    {
        //will have to check which type of unit we are attempting to create
        //setActive of game Object to false first

        if (selectedUnitNum == 1)
        {
            roster.Add(gameController.CreatePlayerArcher());
        }
        else if (selectedUnitNum == 2)
        {
            roster.Add(gameController.CreatePlayerWarrior());
        }
        else if (selectedUnitNum == 3)
        {
            roster.Add(gameController.CreatePlayerMage());
        }
    }

    public void SetSelectedUnitNum(int i)
    {
        selectedUnitNum = i;
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
    void DeployUnits()
    {
        //make a group of the units
        //reset their gameObject's active to true
        gameController.AddFactionGroup(unitsToBeDeployed , transform.position + new Vector3 (5 , 0 , 5) , true);
        unitsToBeDeployed.Clear();

        Debug.Log("Deployed group");
    }

    //return units back to roster
    void ReturnUnits(Group myUnits)
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

    //MORE OF A UI THING MAYBE ADD IT TO UICONTROLLER 
    void PopulateRoster()
    {
        //destroy previous ui buttons
        //get the panel responsible and destroy its children

        //recreate new ones with the ones specified
        for (int i = 0; i < roster.Count; i++)
        {
            //create a new button for each of them
            //HOW DO WE CHECK THEIR TYPE
            if (roster[i].GetClassName() == "WK_ARCHER")
            {
                //we have an archer setup archer info
            }
            else if (roster[i].GetClassName() == "WK_WARRIOR")
            {
                //we have a warrior setup warrior info
            }
            else if (roster[i].GetClassName() == "WK_MAGE")
            {
                //we have a mage setup mage info
            }
        }
    }

    void Update()
    {
        //check gui here 

        // Update building
        building.Update();
    }

    void OnMouseDown()
    {
        if (playerController.buildingToBeBuilt != null) return;

        //deselect everything
        playerController.Deselect();

        //select the building
        playerController.buildingSelected = building;
        Debug.Log("Building selected " + gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        building.OnTriggerEnter();
    }

    void OnTriggerExit(Collider other)
    {
        building.OnTriggerExit();
    }
}
