using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorController : MonoBehaviour {

    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth;

    // Construct the Unit
    public PlayerUnit unit;

    void Awake()
    {
        unit = new PlayerUnit(gameObject, unitHealth);
    }

    void Start()
    {
        unit.playerController.selectedUnit = unit; // temp code 
    }
}
