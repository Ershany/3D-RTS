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
    public List<DynamicUnit> units;
    public List<DynamicUnit> unitsToBeDeployed;

    void Awake()
    {
        //setup members 
        building = new Building(gameObject, buildingHealth);
        units = new List<DynamicUnit>();
        unitsToBeDeployed = new List<DynamicUnit>();

        //Reference game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    void CreateUnit()
    {
    }

    void DeployUnits()
    {
    }

    void ReturnUnits()
    {
    }

    void PopulateRoster()
    {
    }

    void Update()
    {
        //check gui here 
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
        Debug.Log("Enter");
        building.OnTriggerEnter();
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        building.OnTriggerExit();
    }
}
