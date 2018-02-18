using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Prefabs
    public GameObject warriorPrefab;
    public GameObject roguePrefab;
    public GameObject magePrefab;
    public GameObject groupPrefab;

    // References
    private PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }

    void Start ()
    {
        // Make a group, with one of each unit type and assign it to the player
        WarriorController warController = Instantiate(warriorPrefab, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity).GetComponent<WarriorController>();
        RogueController rogueController = Instantiate(roguePrefab, new Vector3(0.0f, 1.0f, 3.0f), Quaternion.identity).GetComponent<RogueController>();
        MageController mageController = Instantiate(magePrefab, new Vector3(3.0f, 1.0f, 0.0f), Quaternion.identity).GetComponent<MageController>();

        Group group = Instantiate(groupPrefab, Vector3.zero, Quaternion.identity, playerController.gameObject.transform).GetComponent<Group>();
        group.AddUnit(warController.unit);
        group.AddUnit(rogueController.unit);
        group.AddUnit(mageController.unit);

        playerController.AddGroup(group);
    }
}
