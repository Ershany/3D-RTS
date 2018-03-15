﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHallController : MonoBehaviour
{
    // Unit Stats
    [Range(1.0f, 1000.0f)] public float buildingHealth = 500.0f;
    public Building building;
    public List<DynamicUnit> units;
    private GameController gameController;
    public List<DynamicUnit> unitsToBeDeployed;

    void Awake()
    {
        //setup members 
        building = new Building(gameObject, buildingHealth);
        units = new List<DynamicUnit>();
        unitsToBeDeployed = new List<DynamicUnit>();

        //Reference game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        //check gui here 
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
