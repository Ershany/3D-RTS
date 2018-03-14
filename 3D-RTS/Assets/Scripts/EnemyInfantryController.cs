using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfantryController : MonoBehaviour
{
    [Range(1.0f, 1000.0f)] public float health;
    public FactionUnit demonUnit;

    // Use this for initialization
    void Awake()
    {
        health = 15.0f;
        int[] stats = { 10, 5, 12};
        demonUnit = new FactionUnit(gameObject, health, stats, false);
    }

    // Update is called once per frame
    void Update()
    {
        demonUnit.Update();
    }
}
