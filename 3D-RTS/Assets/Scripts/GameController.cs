using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Prefabs
    public GameObject warriorPrefab;
    public GameObject roguePrefab;
    public GameObject magePrefab;
    public GameObject groupPrefab;
    public GameObject arenaPrefab;

    // References
    private PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    void Start ()
    {
        // Make a group, with one of each unit type and assign it to the player
        WarriorController warController = Instantiate(warriorPrefab, new Vector3(5.0f, 5.0f, 5.0f), Quaternion.identity).GetComponent<WarriorController>();
        //RogueController rogueController = Instantiate(roguePrefab, new Vector3(0.0f, 1.0f, 3.0f), Quaternion.identity).GetComponent<RogueController>();
        //MageController mageController = Instantiate(magePrefab, new Vector3(3.0f, 1.0f, 0.0f), Quaternion.identity).GetComponent<MageController>();

        WarriorController warController2 = Instantiate(warriorPrefab, new Vector3(16.0f, 16.0f, 16.0f), Quaternion.identity).GetComponent<WarriorController>();
        //RogueController rogueController2 = Instantiate(roguePrefab, new Vector3(12.0f, 12.0f, 12.0f), Quaternion.identity).GetComponent<RogueController>();
        //MageController mageController2 = Instantiate(magePrefab, new Vector3(13.0f, 13.0f, 13.0f), Quaternion.identity).GetComponent<MageController>();

        Group group = Instantiate(groupPrefab, Vector3.zero, Quaternion.identity, playerController.gameObject.transform).GetComponent<Group>();
        group.AddUnit(warController.unit);
        //group.AddUnit(rogueController.unit);
        //group.AddUnit(mageController.unit);
        Group group2 = Instantiate(groupPrefab, new Vector3(0,0,0), Quaternion.identity, playerController.gameObject.transform).GetComponent<Group>();
        group2.AddUnit(warController2.unit);
        //group2.AddUnit(rogueController2.unit);
        //group2.AddUnit(mageController2.unit);

        playerController.AddGroup(group);
        playerController.AddGroup(group2);
    }
}
