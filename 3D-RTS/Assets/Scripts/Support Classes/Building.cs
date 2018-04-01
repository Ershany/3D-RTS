using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building
{
    public enum BuildingType
    {
        GUILDHALL, BLACKSMITH, ARCHERYRANGE, TEMPLEOFMAGI
    }

    public BuildingType Type { get; private set; }
    public bool IsDestroyed { get; private set; }
    public bool IsPlaced { get; private set; }
    public bool isPlayerBuilding;
    public GameObject GameObject { get; private set; }
    public string name;

    private float maxHealth, currentHealth;
    private BoxCollider boxCollider;
    private List<Renderer> renderers;
    private int containingColliderCount;

    public Building(GameObject obj, BuildingType buildingType, float health, string buildingName, bool isPlayer)
    {
        GameObject = obj;
        Type = buildingType;
        renderers = new List<Renderer>();
        boxCollider = GameObject.GetComponent<BoxCollider>();
    
        // Create references to object renderers
        Renderer objRenderer = this.GameObject.GetComponent<Renderer>();
        if (objRenderer != null)
            renderers.Add(this.GameObject.GetComponent<Renderer>());
        foreach(Transform child in this.GameObject.transform)
        {
            if (!child.CompareTag("HeightCheck"))
            {
                renderers.Add(child.gameObject.GetComponent<Renderer>());
            }
        }

        SetHighlightPower(0.5f);
        containingColliderCount = 0;
        maxHealth = currentHealth = health;
        IsDestroyed = false;
        IsPlaced = false;
        isPlayerBuilding = isPlayer;
        name = buildingName;
    }

    public void Update()
    {
        if(!IsPlaced)
        {
            if (IsPlaceableOnTerrain() && containingColliderCount == 0) SetHighlightColor(new Color(0.0f, 1.0f, 0.0f));
            else SetHighlightColor(new Color(1.0f, 0.0f, 0.0f));
        }
    }

    //damage building
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            IsDestroyed = true;
        }
    }

    //repair building
    public void RepairDamage(float repairAmount)
    {
        currentHealth += repairAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    //move building to a certain position (before placing on terrain)
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
            if (child.CompareTag("HeightCheck") && Terrain.activeTerrain.SampleHeight(child.position) != System.Math.Round(child.position.y, 2))
            {
                return false;
            }
        }

        return true;
    }

    // Attempts to place the building on the terrain. Will return true if successful
    public bool PlaceBuildingOnTerrain()
    {
        if (IsPlaceableOnTerrain() && containingColliderCount == 0)
        {
            // Place building on terrain
            boxCollider.isTrigger = false;
            NavMeshObstacle obstacle = GameObject.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            IsPlaced = true;
            Rigidbody rb = this.GameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.isKinematic = true;
            SetHighlightPower(0.0f);
            this.GameObject.layer = 0;

            SetHighlightPower(0.0f);

            return true;
        }

        Debug.Log("Can't place building - Current contained collider count: " + containingColliderCount);
        return false;
    }

    //Entering collision detection 
    public void OnTriggerEnter()
    {
        ++containingColliderCount;
        Debug.Log(containingColliderCount);
    }


    //Exiting collision detection
    public void OnTriggerExit()
    {
        --containingColliderCount;
        Debug.Log(containingColliderCount);
    }

    //Set the color of the highlight before placing the building down
    private void SetHighlightColor(Color highlightColor)
    {
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetColor("_HighlightColor", highlightColor);
            }
        }
    }

    // Value should be in the following range: [0, 1]
    private void SetHighlightPower(float value)
    {
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetFloat("_HighlightPower", value);
            }
        }
    }
}
