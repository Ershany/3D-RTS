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

    public void GroupTakeDamage(float damage)
    {
        foreach(DynamicUnit unit in units)
        {
            unit.TakeDamage(damage);
        }
    }

    public void GroupHealDamage(float healAmount)
    {
        foreach (DynamicUnit unit in units)
        {
            unit.HealDamage(healAmount);
        }
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

    public void BattleStarted()
    {
        foreach(DynamicUnit unit in units)
        {
            
            unit.IsInBattle = true;
        }
    }


    public void BattledEnded()
    {
        foreach (DynamicUnit unit in units)
        {
            unit.IsInBattle = false;
        }
    }


    public List<DynamicUnit> GetUnits()
    {
        return units;
    }

    public DynamicUnit GetFirstUnit()
    {
        return units[0];
    }
}
