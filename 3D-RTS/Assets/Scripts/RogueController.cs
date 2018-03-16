using System.Collections;
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
        int[] stats = new int[3];
        stats[0] = 4;
        stats[1] = 6;
        stats[2] = 8;
        unit = new FactionUnit(gameObject, unitHealth, stats, true, "WK_ARCHER");
    }

    void Update()
    {
        unit.Update();
    }
}
