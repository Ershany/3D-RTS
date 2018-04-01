using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class EnvironmentUnits
{
    //store prefabs for static units here
    public static List<GameObject> staticUnits;
}

public class RandomEncounterController : MonoBehaviour
{
    public StaticUnitController staticUnit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //atm what i am gonna do is check it in update
        if (staticUnit == null) return;

        //check if it is not in battle if it is not destroy the prefab
        //if the static unit won the battle remove it
        if (!staticUnit.unit.IsInBattle)
        {
            Destroy(staticUnit.unit.GetGameObject());
        }
    }

    //on collision check for random encounter
    private void OnCollisionEnter(Collision collision)
    {
        int random = Random.Range(0, 7);

        //50% chance of random encounter
        if (random > 3)
        {
            //create the units at the position of the collision
            FoeEncounter(collision.transform);
            Debug.Log("u have encountered an ememy unit");
        }
        else
        {
            Debug.Log("u are fine u have survived this encounter");
        }
    }

    //setup a foe encounter
    void FoeEncounter(Transform trans)
    {
        //instantiate a random static unit
        int index = Random.Range(0 , EnvironmentUnits.staticUnits.Count - 1);
        GameObject unit = Instantiate(EnvironmentUnits.staticUnits[index], trans.position, Quaternion.identity);

        //I think the look at stuff is done in the battle
        //unit.transform.LookAt();

        staticUnit = unit.GetComponent<StaticUnitController>();
    }
}
