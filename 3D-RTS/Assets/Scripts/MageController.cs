﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {

    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth = 10.0f;

    // Construct the Unit
    public PlayerUnit unit;

    void Awake()
    {
        unit = new PlayerUnit(gameObject, unitHealth);
    }

    void Update()
    {
        unit.AnimateUnit();    
    }
}
