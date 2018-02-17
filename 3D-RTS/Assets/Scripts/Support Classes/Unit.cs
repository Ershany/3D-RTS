using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Unit {

    protected float maxHealth, currentHealth;
    public bool IsDead { get; protected set; }

	public Unit(float health)
    {
        maxHealth = currentHealth = health;
        IsDead = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            IsDead = true;
        }
    }
}
