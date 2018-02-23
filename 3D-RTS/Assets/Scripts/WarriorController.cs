using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : MonoBehaviour {

    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth = 20.0f;

    // Construct the Unit
    public PlayerUnit unit;

    void Awake()
    {
        unit = new PlayerUnit(gameObject, unitHealth);
    }

    void Update()
    {
        unit.Update();
    }
}
