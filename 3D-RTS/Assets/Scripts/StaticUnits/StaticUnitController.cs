using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUnitController : MonoBehaviour
{
    //do not use these variables it is for the prefabs in the inspector
    public float initialHealth;
    public float initialDamage;
    public string initialName;

    public StaticUnit unit;

    //since it is static it doesn't have a navMesh it will just do animations

    // Use this for initialization
    void Start()
    {
        unit = new StaticUnit(gameObject, initialHealth, initialDamage , initialName);
    }

    void Update()
    {
        //THIS MIGHT NEED TO HAPPEN IN THE BATTLE ITSELF BUT JUST IN CASE .....
        if (unit.IsDead)
        {
            //destroy the gameObject
            Destroy(gameObject);
        }
    }
}
