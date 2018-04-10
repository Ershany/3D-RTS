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
        demonUnit = new FactionUnit(gameObject, false , "EnemyWarrior");
    }

    // Update is called once per frame
    void Update()
    {
        demonUnit.Update();
    }
}
