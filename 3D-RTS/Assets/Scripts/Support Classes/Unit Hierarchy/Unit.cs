using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Unit {

    protected float maxHealth, currentHealth;
    public bool IsDead { get; protected set; }
    protected GameObject gameObject;

	public Unit(GameObject obj, float health)
    {
        gameObject = obj;
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

    public void SetGameObjectParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }
}
