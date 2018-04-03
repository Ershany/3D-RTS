using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class DynamicUnit : Unit
{
    public bool IsPlayerControlled { get; protected set; }
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Rigidbody rb;

    protected int dexterity;
    protected int strength;
    protected int intelligence;
    protected int maxMana;
    protected int currentMana;
    protected int level;
    protected int experience;

    protected float health_growth;
    protected float mana_growth;
    protected float dexterity_growth;
    protected float strength_growth;
    protected float intelligence_growth;
    
    protected string name;
    protected string className;
    public float attackTime { get; set; }
    public bool IsAttacking { get; protected set; }
    public bool IsInBattle { get; set; }
    protected DynamicUnit targetUnit;
    protected List<DynamicUnit> opponents;

    //used for now 
    public Vector3 destination;

    public DynamicUnit(GameObject obj, float health, bool playerControlled , string className) : base(obj, health)
    {
        IsPlayerControlled = playerControlled;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        IsAttacking = false;
        IsInBattle = false;
        this.className = className;
    }

    //these are probably for the useless units that are used for gui
    public DynamicUnit(string className)
    {
        IsPlayerControlled = false;
        IsAttacking = false;
        IsInBattle = false;
        StatKey.GetStats(className, ref maxHealth, ref maxMana, ref strength, ref intelligence, ref dexterity,
                         ref health_growth, ref mana_growth, ref strength_growth, ref intelligence_growth,  ref dexterity_growth);
        currentHealth = maxHealth;
        currentMana = maxMana;
        level = 1;
        experience = 0;
        name = "NAME";
        this.className = "WK_" + className;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
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
        return dexterity + Mathf.FloorToInt(level * dexterity_growth);
    }
    public int GetStrength()
    {
        return strength + Mathf.FloorToInt(level * strength_growth);
    }
    public int GetIntelligence()
    {
        return intelligence + Mathf.FloorToInt(level * intelligence_growth);
    }

    public int GetLevel()
    {
        if (experience >= GetExperienceRequired())
        {
            experience -= GetExperienceRequired();
            level++;
        }
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

    public void GiveExp(int xp)
    {
        experience += xp;
        if (experience >= GetExperienceRequired())
        {
            experience -= GetExperienceRequired();
            level++;
        }
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetExperienceRequired()
    {
        return 100 + (30 * (level - 1));
    }

    public void PermaBuffMaxHealth(float healthBuff)
    {
        //find previous ratio of health
        float healthRatio = currentHealth / maxHealth;

        maxHealth += healthBuff;
       
        //resetup ratio of current health to max health
        currentHealth = maxHealth * healthRatio;
    }

    public void PermaBuffMaxStrength(int strengthBuff)
    {
        strength += strengthBuff;
    }

    public void PremaBuffMaxMana(int manaBuff)
    {
        // find mana ratio before adding health
        int manaRatio = currentMana / maxMana;

        maxMana += manaBuff;

        // resetup current mana after buff
        currentMana = maxMana * manaRatio;
    }

    public void PermaBuffMaxIntelligence(int intelligenceBuff)
    {
        intelligence += intelligenceBuff;
    }

    public void PermaBuffMaxDexterity(int dexterityBuff)
    {
        dexterity += dexterityBuff;
    }
}
