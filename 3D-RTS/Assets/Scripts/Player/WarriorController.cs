using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : MonoBehaviour
{
    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth = 20.0f;

    // Construct the Unit
    public FactionUnit unit;

    void Awake()
    {
        unit = new FactionUnit(gameObject, true, "Warrior");
    }

    void Update()
    {
        unit.Update();
    }
}
