using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentUnits
{
    //store prefabs for static units here
    public static  List<GameObject> staticUnits;
}

public class RandomEncounterController : MonoBehaviour
{
    public StaticUnitController staticUnit;
    public bool battleEncountered;

	// Use this for initialization
	void Start ()
    {
        battleEncountered = false;
	}
	
	//LateUpdate just in case
	void LateUpdate ()
    {
        //atm what i am gonna do is check it in update
        if (staticUnit == null)
        {
            battleEncountered = false;
            return;
        }

        //check if it is not in battle if it is not destroy the prefab
        //if the static unit won the battle remove it
        if (!staticUnit.unit.IsInBattle)
        {
            battleEncountered = false;
            Destroy(staticUnit.unit.GetGameObject());
        }
    }

    //on collision check for random encounter
    void OnTriggerEnter(Collider other)
    {
        if (battleEncountered) return;

        int random = Random.Range(0, 7);

        //12.5% chance of random encounter (I think)
        if (random > 1)
        {
            FoeEncounter(other.transform);
            Debug.Log("u have encountered an ememy unit");
            battleEncountered = true;
        }
        else { Debug.Log("u are fine u have survived this encounter"); }
    }

    //setup a foe encounter
    void FoeEncounter(Transform trans)
    {
        //instantiate a random static unit
        int index = Random.Range(0 , EnvironmentUnits.staticUnits.Count - 1);
        GameObject unit = Instantiate(EnvironmentUnits.staticUnits[index], trans.position, Quaternion.identity);

        staticUnit = unit.GetComponent<StaticUnitController>();
        Debug.Log(staticUnit.name);
    }
}
