using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

    private float maxHealth, currentHealth;
    public bool IsDestroyed { get; private set; }
    private GameObject gameObject;

    public Building(GameObject obj, float health)
    {
        gameObject = obj;
        maxHealth = currentHealth = health;
        IsDestroyed = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            IsDestroyed = true;
        }
    }

    public void RepairDamage(float repairAmount)
    {
        currentHealth += repairAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Checks if the building in its current position, is allowed to be placed on the terrain (not on sloped terrain)
    public bool IsPlaceableOnTerrain()
    {
        // Loop through all of the children's positions and make sure they are on the same level of elevation
        bool firstIter = true;
        float terrainHeight = 0.0f;
        foreach (Transform child in gameObject.transform)
        {
            float currentHeight = Terrain.activeTerrain.SampleHeight(child.position);
            if (firstIter)
            {
                terrainHeight = currentHeight;
                firstIter = false;
            }
            if (terrainHeight != currentHeight) return false;
        }
        return true;
    }
}
