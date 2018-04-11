using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestState : IState {

    private EnemyController controller;
    private PlayerController playerController;
    private Transform guildhallLocation;

    private Transform defenseLocation;
    private float wanderTime, currentTime;

    public HarvestState(GameObject controller)
    {
        currentTime = wanderTime = 10.0f;
        this.controller = controller.GetComponent<EnemyController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        defenseLocation = GameObject.FindGameObjectWithTag("DefenseLoc").transform;

        guildhallLocation = null;
    }

    void IState.Update()
    {
        // Wander using a timer (wanderTime)
        currentTime += Time.deltaTime;
        if (currentTime >= wanderTime)
        {
            foreach (Group group in controller.enemyGroups)
            {
                if (group.GetFirstUnit().IsInBattle) continue;
                group.SetGroupDestination(GetRandomLoc(), null);
            }
            currentTime -= wanderTime;
        }

        // Check for state transition (Harvet -> Defense)
        foreach (Group playerGroup in controller.playerController.groups)
        {
            float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - defenseLocation.position).sqrMagnitude;
            if (distanceSqr < OffenseState.attackRangeSqr)
            {
                Debug.Log("Harvest -> Defense State");
                EnemyController.defenseState.SetDefenseTime(30.0f);
                controller.State = EnemyController.defenseState;
            }
        }

        // Check for state transition (Harvest -> Offense)
        foreach (Group group in controller.enemyGroups)
        {
            if (group.GetFirstUnit().IsInBattle) continue;

            // Fighting seek
            foreach (Group playerGroup in controller.playerController.groups)
            {
                float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < OffenseState.attackRangeSqr)
                {
                    Debug.Log("Harvest -> Offense State");
                    controller.State = EnemyController.offenseState;
                }
            }

            // Guild hall seek
            if (playerController.guildHallBuilt && guildhallLocation == null)
            {
                guildhallLocation = GameObject.FindGameObjectWithTag("GuildHall").transform;
            }
            if (guildhallLocation != null)
            {
                float distanceSqr = (guildhallLocation.position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < OffenseState.attackRangeSqr)
                {
                    Debug.Log("Harvest -> Offense State");
                    controller.State = EnemyController.offenseState;
                }
            }
        }
    }

    private Vector3 GetRandomLoc()
    {
        return new Vector3(Random.Range(0.0f, 512.0f), 0.0f, Random.Range(0.0f, 512.0f));
    }

}
