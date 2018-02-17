using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DemonUnit : DynamicUnit {

	public DemonUnit(float health) : base(health, false)
    {

    }
}
