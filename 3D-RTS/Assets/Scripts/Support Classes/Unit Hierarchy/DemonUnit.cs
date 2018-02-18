using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class DemonUnit : DynamicUnit {

	public DemonUnit(GameObject obj, float health) : base(obj, health, false)
    {

    }
}
