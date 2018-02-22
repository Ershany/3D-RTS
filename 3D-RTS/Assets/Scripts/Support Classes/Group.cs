using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    private List<DynamicUnit> units;

    void Awake()
    {
        units = new List<DynamicUnit>();
    }

    public void AddUnit(DynamicUnit unit)
    {
        unit.SetGameObjectParent(this.transform);
        units.Add(unit);
    }

    public void SetGroupDestination(Vector3 dest)
    {
        foreach(DynamicUnit unit in units)
        {
            unit.SetDestination(dest);
        }
    }

    public void StopGroupMovement()
    {
        foreach(DynamicUnit unit in units)
        {
            unit.StopMovement();
        }
    }

    public DynamicUnit GetFirstUnit()
    {
        return units[0];
    }
}
