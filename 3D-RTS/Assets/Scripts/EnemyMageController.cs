using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMageController : MonoBehaviour
{
    [Range(1.0f, 1000.0f)] public float health;
    public DemonUnit demonUnit;

    // Use this for initialization
    void Awake()
    {
        health = 15.0f;
        demonUnit = new DemonUnit(gameObject, health);
    }

    // Update is called once per frame
    void Update()
    {
        demonUnit.Update();
    }
}
