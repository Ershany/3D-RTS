using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PlayerUnit : DynamicUnit {

    public PlayerController playerController;

	public PlayerUnit(GameObject obj, float health) : base(obj, health, true)
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
    }
}
