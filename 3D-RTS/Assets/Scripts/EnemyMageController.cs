﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMageController : MonoBehaviour
{
    [Range(1.0f, 1000.0f)] public float health;
    public PlayerUnit demonUnit;

    // Use this for initialization
    void Awake()
    {
        health = 20.0f;
        int[] stats = { 8, 5, 12 };
        demonUnit = new PlayerUnit(gameObject, health, stats);
    }

    // Update is called once per frame
    void Update()
    {
        demonUnit.Update();
    }
}
