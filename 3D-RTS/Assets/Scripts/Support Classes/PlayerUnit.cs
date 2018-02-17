using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerUnit : DynamicUnit {

	public PlayerUnit(float health) : base(health, true)
    {
        
    }
}
