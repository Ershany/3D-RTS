using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHallController : MonoBehaviour
{
    // Unit Stats
    [Range(1.0f, 1000.0f)] public float buildingHealth = 500.0f;

    public Building building;
    

    void Awake()
    {
        building = new Building(gameObject, buildingHealth);
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
