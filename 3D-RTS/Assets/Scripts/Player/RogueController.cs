﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueController : MonoBehaviour
{
    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth = 15.0f;

    // Construct the Unit
    public FactionUnit unit;

    void Awake()
    {
        unit = new FactionUnit(gameObject, true, "Archer");
    }

    void Update()
    {
        unit.Update();
    }
}
