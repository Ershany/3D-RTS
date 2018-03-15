﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class DynamicUnit : Unit
{
    public bool IsPlayerControlled { get; protected set; }
    protected NavMeshAgent agent;
    protected Animator anim;

    protected int dexterity;
    protected int strength;
    protected int intelligence;
    protected int maxMana;
    protected int currentMana;
    protected int level;
    protected int experience;

    protected string name;
    protected string className;
    public float attackTime { get; set; }
    public bool IsAttacking { get; protected set; }
    public bool IsInBattle { get; set; }
    protected DynamicUnit targetUnit;
    protected List<DynamicUnit> opponents;

    public DynamicUnit(GameObject obj, float health, bool playerControlled , string className) : base(obj, health)
    {
        IsPlayerControlled = playerControlled;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        IsAttacking = false;
        IsInBattle = false;
        this.className = className;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }

    public void Halt()
    {
        agent.SetDestination(GetTransform().position);
    }


    public void BeginAttack(List<DynamicUnit> targets)
    {
        opponents = targets;
    }

    public void AttackAnyEnemy()
    {
        IsAttacking = true;
        for (int i = 0; i < opponents.Count; i++)
        {
            if (!opponents[i].IsDead)
            {
                targetUnit = opponents[i];

                Vector3 offsetVector = Vector3.Normalize(opponents[i].GetTransform().position - GetTransform().position);
                float scale = 4.0f;
                Vector3 targetPosition = opponents[i].GetTransform().position - scale * offsetVector;
                SetDestination(targetPosition);
                break;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            IsDead = true;
        }
        else
            anim.SetBool("TakeDamage", true);
    }

    public int GetDexterity()
    {
        return dexterity;
    }
    public int GetStrength()
    {
        return strength;
    }
    public int GetIntelligence()
    {
        return intelligence;
    }

    public int GetLevel()
    {
        return level;
    }
    
    public void GetMana(ref int curr, ref int max)
    {
        curr = currentMana;
        max = maxMana;
    }

    public void GetHealth(ref float curr, ref float max)
    {
        curr = currentHealth;
        max = maxHealth;
    }


    public void GetStatus(ref int str, ref int intel, ref int dex, ref float mHP, ref float currHP, ref int mMP, ref int currMP, ref int lvl)
    {
        str = GetStrength();
        intel = GetIntelligence();
        dex = GetDexterity();
        GetHealth(ref currHP,ref mHP);
        GetMana(ref currMP, ref mMP);
        lvl = GetLevel();
        
    }
    public string GetName()
    {
       return name;
    }

    public string GetClassName()
    {
        return className;
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetExperienceRequired()
    {

        return level * 100;
    }


}
