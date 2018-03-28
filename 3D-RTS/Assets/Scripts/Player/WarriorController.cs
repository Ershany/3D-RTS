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
        int[] stats = new int[3];
        stats[0] = 8;
        stats[1] = 2;
        stats[2] = 5;
        unit = new FactionUnit(gameObject, unitHealth, stats, true, "WK_WARRIOR");
    }

    void Update()
    {
        unit.Update();
    }
}
