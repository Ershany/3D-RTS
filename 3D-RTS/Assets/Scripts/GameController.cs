using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Prefabs
    public GameObject warriorPrefab;
    public GameObject roguePrefab;
    public GameObject magePrefab;
    public GameObject groupPrefab;
    public GameObject arenaPrefab;

    // References
    private PlayerController playerController;
    private EnemyController enemyController;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
    }

    void Start()
    {
        // Make a group, with one of each unit type and assign it to the player
        WarriorController warController = Instantiate(warriorPrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<WarriorController>();
        WarriorController warControllera = Instantiate(warriorPrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<WarriorController>();
        WarriorController warControllerb = Instantiate(warriorPrefab, new Vector3(15.0f, 2.0f, 15.0f), Quaternion.identity).GetComponent<WarriorController>();
        //RogueController rogueController = Instantiate(roguePrefab, new Vector3(0.0f, 1.0f, 3.0f), Quaternion.identity).GetComponent<RogueController>();
        //MageController mageController = Instantiate(magePrefab, new Vector3(3.0f, 1.0f, 0.0f), Quaternion.identity).GetComponent<MageController>();

        WarriorController warController2 = Instantiate(warriorPrefab, new Vector3(26.0f, 2.0f, 26.0f), Quaternion.identity).GetComponent<WarriorController>();
        WarriorController warController2a = Instantiate(warriorPrefab, new Vector3(27.0f, 2.0f, 26.0f), Quaternion.identity).GetComponent<WarriorController>();
        WarriorController warController2b = Instantiate(warriorPrefab, new Vector3(28.0f, 2.0f, 26.0f), Quaternion.identity).GetComponent<WarriorController>();
        //RogueController rogueController2 = Instantiate(roguePrefab, new Vector3(12.0f, 12.0f, 12.0f), Quaternion.identity).GetComponent<RogueController>();
        //MageController mageController2 = Instantiate(magePrefab, new Vector3(13.0f, 13.0f, 13.0f), Quaternion.identity).GetComponent<MageController>();

        Group group = Instantiate(groupPrefab, Vector3.zero, Quaternion.identity, playerController.gameObject.transform).GetComponent<Group>();
        group.AddUnit(warController.unit);
        group.AddUnit(warControllera.unit);
        group.AddUnit(warControllerb.unit);
        //group.AddUnit(rogueController.unit);
        //group.AddUnit(mageController.unit);
        Group group2 = Instantiate(groupPrefab, new Vector3(0, 0, 0), Quaternion.identity, playerController.gameObject.transform).GetComponent<Group>();
        group2.AddUnit(warController2.unit);
        group2.AddUnit(warController2a.unit);
        group2.AddUnit(warController2b.unit);
        //group2.AddUnit(rogueController2.unit);
        //group2.AddUnit(mageController2.unit);

        playerController.AddGroup(group);
        playerController.AddGroup(group2);
    }

    void Update()
    {
        //check for battles
        for (int i = 0; i < playerController.groups.Count; i++)
        {
            //if we a group is in a battle leave
            //might need to change this
            if (playerController.groups[i].GetFirstUnit().IsInBattle) continue;

            for (int j = 0; j < enemyController.enemyGroups.Count; j++)
            {
                if (Vector3.Distance(playerController.groups[i].GetUnits()[i].GetTransform().position, enemyController.enemyGroups[j].GetUnits()[j].GetTransform().position) < 4)
                {
                    GameObject arena = Instantiate(arenaPrefab, playerController.groups[i].GetUnits()[i].GetTransform().position, Quaternion.identity);
                    
                    //position of battle, 2 groups in conflict, and arena asset 
                    playerController.battles.Add(new TurnBasedBattleController(new Vector3(playerController.groups[i].GetUnits()[i].GetTransform().position.x, 0.0f, playerController.groups[i].GetUnits()[i].GetTransform().position.z), playerController.groups[i], enemyController.enemyGroups[j], arena));
                    
                    //i = 4;

                    //start battle for both units 
                    playerController.groups[i].BattleStarted();
                    enemyController.enemyGroups[j].BattleStarted();
                    break;
                }
            }
        }
    }
}
