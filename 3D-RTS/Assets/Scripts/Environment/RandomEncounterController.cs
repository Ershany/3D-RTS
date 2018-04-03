using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentUnits
{
    //store prefabs for static units here
    public static List<GameObject> neutralUnits;
}

public class RandomEncounterController : MonoBehaviour
{
    public List<List<NeutralUnitController>> neutralUnits;
    public GameController gameCon;
    public PlayerController playCon;
    public bool battleEncountered;
    public List<Group> battleGroups;

    public List<Group> objectsToIgnore;

    // Use this for initialization
    void Start()
    {
        neutralUnits = new List<List<NeutralUnitController>>();
        battleEncountered = false;
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playCon = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        objectsToIgnore = new List<Group>();
    }

    //LateUpdate just in case
    public void UpdateRandomBattleController()
    {
        if (battleGroups.Count < 1)
            battleEncountered = false;

        for (int i = 0; i < playCon.groups.Count; i++)
        {
            if (objectsToIgnore.Contains(playCon.groups[i]))
            {
                if (!playCon.groups[i].GetFirstUnit().IsInBattle)
                {
                    if(Random.Range(0,10) > 6)
                    {
                        objectsToIgnore.Remove(playCon.groups[i]);
                    }
                }
                continue;
            }
            for (int j = 0; j < playCon.groups[i].GetUnits().Count; j++)
            {
                int random = Random.Range(0, 7);

                if (random > 5)
                {
                    if (gameObject.GetComponent<Collider>().bounds.Contains(playCon.groups[i].GetUnits()[j].GetTransform().position))
                    {
                        Debug.Log("Foe encountered");
                        FoeEncounter(playCon.groups[i].GetUnits()[j].GetTransform());
                        battleEncountered = true;
                        objectsToIgnore.Add(playCon.groups[i]);
                        battleGroups.Add(playCon.groups[i]);
                        j = 4;
                    }

                }
            }
        }



    }

    ////on collision check for random encounter
    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("u have encountered an ememy unit");
    //    for (int i = 0; i < objectsToIgnore.Count; i++)
    //    {
    //        if(other.gameObject == objectsToIgnore[i].gameObject)
    //        {
    //            Debug.Log("worked");
    //            return;
    //        }
    //    }
    //    int random = Random.Range(0, 7);

    //    //12.5% chance of random encounter (I think)
    //    if (random > 1)
    //    {
    //        Debug.Log("u have encountered an ememy unit");
    //        FoeEncounter(other.transform);
    //        battleEncountered = true;
    //        objectsToIgnore.AddRange(other.transform.parent.GetComponentsInChildren<Transform>());
    //    }
    //}

    //setup a foe encounter
    void FoeEncounter(Transform trans)
    {
        //Use to control number of enemies in a random encounter
        int enemyCount = Random.Range(1, 2);
        Debug.Log("enemyCount: " + enemyCount);
        //instantiate a random static unit
        List<NeutralUnitController> newBattleGroup = new List<NeutralUnitController>();
        for (int i = 0; i < enemyCount; i++)
        {
            int index = Random.Range(0, EnvironmentUnits.neutralUnits.Count - 1);
            // unit.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            newBattleGroup.Add(Instantiate(EnvironmentUnits.neutralUnits[0], trans.position, Quaternion.identity).GetComponent<NeutralUnitController>());

            Debug.Log(newBattleGroup[i].name);
        }
        neutralUnits.Add(newBattleGroup);
    }

    public List<DynamicUnit> GetUnits(int j)
    {
        List<DynamicUnit> temp = new List<DynamicUnit>();

        for (int i = 0; i < neutralUnits[j].Count; i++)
        {
            temp.Add(neutralUnits[j][i].GetUnit());
        }

        return temp;
    }
}
