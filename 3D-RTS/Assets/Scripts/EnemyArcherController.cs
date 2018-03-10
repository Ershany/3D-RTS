﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherController : MonoBehaviour
{
    [Range(1.0f, 1000.0f)] public float health;
    public PlayerUnit demonUnit;

    // Use this for initialization
    void Awake()
    {
        health = 10.0f;
        int[] stats = { 15, 6, 12 };
        demonUnit = new PlayerUnit(gameObject, health, stats);
    }

    // Update is called once per frame
    void Update()
    {
        demonUnit.Update();
    } 
}


   