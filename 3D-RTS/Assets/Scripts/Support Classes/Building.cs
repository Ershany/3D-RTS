using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building {

    private float maxHealth, currentHealth;
    public bool IsDestroyed { get; private set; }
    public bool IsPlaced { get; private set; }
    public GameObject GameObject { get; private set; }
    private BoxCollider boxCollider;
    private Renderer renderer;

    public Building(GameObject obj, float health)
    {
        GameObject = obj;
        boxCollider = GameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        renderer = GameObject.GetComponent<Renderer>();

        maxHealth = currentHealth = health;
        IsDestroyed = false;
        IsPlaced = false;
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

    public void MoveBuilding(Vector3 newPos)
    {
        GameObject.transform.position = newPos;
    }

    // Checks if the building in its current position, is allowed to be placed on the terrain (not on sloped terrain)
    private bool IsPlaceableOnTerrain()
    {
        if (IsPlaced) return true;
        // Loop through all of the children's positions and make sure they are on the terrain
        foreach (Transform child in GameObject.transform)
        {
            // Round the height, due to float precision errors
            if (child.CompareTag("HeightCheck") && Terrain.activeTerrain.SampleHeight(child.position) != System.Math.Round(child.position.y, 2)) return false;
        }
        return true;
    }

    // Attempts to place the building on the terrain. Will return true if successful
    public bool PlaceBuildingOnTerrain()
    {
        if (IsPlaceableOnTerrain())
        {
            boxCollider.enabled = true;
            NavMeshObstacle obstacle = GameObject.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;

            return true;
        }
        return false;
    }
}
