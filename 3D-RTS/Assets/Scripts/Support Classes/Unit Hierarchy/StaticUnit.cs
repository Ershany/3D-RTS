using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaticUnit : Unit
{
    public Animator anim;   //for animations
    public float unitDamage;

    protected string name;
    public float attackTime { get; set; }
    public bool IsAttacking { get; protected set; }
    public bool IsInBattle { get; set; }
    protected DynamicUnit targetUnit;
    protected List<DynamicUnit> opponents;

    public StaticUnit(GameObject obj, float health , float damage , string name) : base(obj, health)
    {
        IsInBattle = false;
        unitDamage = damage;
        this.name = name;
        anim = obj.GetComponent<Animator>();
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    //Check animations here and check for death 
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            IsDead = true;
        }
        else
        {
            anim.SetBool("TakeDamage", true);
        }
    }
}
