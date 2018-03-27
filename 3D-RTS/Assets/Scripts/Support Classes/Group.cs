using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    private List<DynamicUnit> units;
    private bool isInBattle;
    public bool returningToGuildHall;
    public Vector3 rawDestination;
    void Awake()
    {
        units = new List<DynamicUnit>();
        isInBattle = false;
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
        rawDestination = dest;
        foreach(DynamicUnit unit in units)
        {
            unit.SetDestination(dest);
        }
    }
    public void ResetGroupDestination()
    {
        foreach (DynamicUnit unit in units)
        {
            unit.SetDestination(rawDestination);
        }
    }

    public void RemoveBumperCars()
    {
        foreach (DynamicUnit unit in units)
        {
            unit.GetAgent().radius = 0.0f;
        }
    }
    public void ReInitializeBumperCars()
    {
        foreach (DynamicUnit unit in units)
        {
            unit.GetAgent().radius = 0.5f;
        }
    }
    public void DisableCollisionsWithCollider(Collider col)
    {
        foreach(DynamicUnit unit in units)
        {
            Physics.IgnoreCollision(unit.GetGameObject().GetComponent<Collider>(), col, true);
        }
    }

    public void EnableCollisionsWithCollider(Collider col)
    {
        foreach (DynamicUnit unit in units)
        {
            Physics.IgnoreCollision(unit.GetGameObject().GetComponent<Collider>(), col, false);
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
        isInBattle = true;
        foreach(DynamicUnit unit in units)
        {
            unit.IsInBattle = true;
        }
    }


    public void BattledEnded()
    {
        isInBattle = false;
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
