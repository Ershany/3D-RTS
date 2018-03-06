using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {

    // Unit Stats
    [Range(1.0f, 1000.0f)] public float unitHealth = 10.0f;

    // Construct the Unit
    public PlayerUnit unit;

    void Awake()
    {
        int[] stats = new int[3];
        stats[0] = 2;
        stats[1] = 8;
        stats[2] = 4;
        unit = new PlayerUnit(gameObject, unitHealth, stats);
        
    }

    void Update()
    {
        unit.Update();    
    }
}
