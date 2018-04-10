using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    private List<DynamicUnit> units;
    public bool returningToGuildHall;
    public Vector3 rawDestination;
    private Transform dynamicDestination;

    public float randomBattleBufferTimer;

    private Vector3 patrolPoint1, patrolPoint2;
    private bool patrolling;
    void Awake()
    {
        randomBattleBufferTimer = 0;
        patrolling = false;
        patrolPoint1 = patrolPoint2 = new Vector3(-1, -1, -1);
        units = new List<DynamicUnit>();
        dynamicDestination = null;
    }

    void Update()
    {
        if (randomBattleBufferTimer > 0.0f)
            randomBattleBufferTimer -= Time.deltaTime;
        if (dynamicDestination != null)
        {
            SetGroupDestination(dynamicDestination.position, dynamicDestination);
        }
        if (patrolling)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if(Vector3.Distance(units[i].GetTransform().position, units[i].GetAgent().destination) < 5)
                {
                    if (rawDestination == patrolPoint1)
                        SetGroupDestination(patrolPoint2, null);
                    else
                        SetGroupDestination(patrolPoint1, null);
                }
            }
        }

        CullDead();
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

    public void SetGroupDestination(Vector3 dest, Transform dynDest)
    {
        patrolling = false;
        dynamicDestination = dynDest;
        rawDestination = dest;
        foreach(DynamicUnit unit in units)
        {
            unit.SetDestination(dest);
        }
    }
    public void SetGroupPatrol(Vector3 dest1, Vector3 dest2)
    {

        patrolling = true;
        SetGroupDestination(dest1, null);
        patrolPoint1 = dest1;
        patrolPoint2 = dest2;

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
        patrolling = false;
        foreach(DynamicUnit unit in units)
        {
            unit.StopMovement();
        }
    }

    public void BattleStarted()
    {
        patrolling = false;
        foreach (DynamicUnit unit in units)
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

    public void CullDead()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].IsDead && units[i].GetCurrentHealth() < 0)
            {
                DynamicUnit temp = units[i];
                units.Remove(units[i]);
                Destroy(temp.GetGameObject());
            }
        }
    }

    public DynamicUnit GetFirstUnit()
    {
        return units[0];
    }
}