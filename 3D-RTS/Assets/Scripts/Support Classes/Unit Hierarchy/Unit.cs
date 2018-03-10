using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Unit
{

    public bool IsDead { get; protected set; }


    protected float maxHealth, currentHealth;
    protected GameObject gameObject;
    protected CapsuleCollider collider;


    public Unit(GameObject obj, float health)
    {
        gameObject = obj;
        collider = gameObject.GetComponent<CapsuleCollider>();
        maxHealth = currentHealth = health;
        IsDead = false;
    }


    public void HealDamage(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetGameObjectParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }




}
