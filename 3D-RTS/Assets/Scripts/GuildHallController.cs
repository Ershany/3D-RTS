using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHallController : MonoBehaviour {

    // Unit Stats
    [Range(1.0f, 1000.0f)] public float buildingHealth = 500.0f;

    // Construct the Unit
    public Building building;

    void Awake()
    {
        building = new Building(gameObject, buildingHealth, transform.localScale.y);
    }
}
