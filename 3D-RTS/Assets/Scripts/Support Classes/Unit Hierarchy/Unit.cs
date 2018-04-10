using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Unit
{
    public bool IsDead { get; protected set; }
    protected float maxHealth, currentHealth;
    protected GameObject gameObject { get; private set; }
    protected CapsuleCollider collider;

    public Unit(GameObject obj)
    {
        gameObject = obj;
        collider = gameObject.GetComponent<CapsuleCollider>();
        IsDead = false;
    }

    public Unit()
    {

    }

    public Unit(float health)
    {
        gameObject = null;
        maxHealth = currentHealth = health;
    }

    public void HealDamage(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetUninitiatedGameObject(GameObject _gameObject)
    {
        if (gameObject == null)
        {
            gameObject = _gameObject;
        }
        else
        {
            Debug.Log("gameObject not null");
        }
    }

    public void SetGameObjectParent(Transform parent)
    {
        gameObject.transform.SetParent(parent,false);
    }
}
