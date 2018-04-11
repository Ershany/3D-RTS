using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseState : IState {

    private EnemyController controller;
    private PlayerController playerController;
    private Transform guildhallLocation;

    private Transform defenseLocation;
    private float defenseTime, currentTime;

    public DefenseState(GameObject controller)
    {
        currentTime = 0.0f;
        defenseTime = 10.0f;
        this.controller = controller.GetComponent<EnemyController>();
        defenseLocation = GameObject.FindGameObjectWithTag("DefenseLoc").transform;
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        guildhallLocation = null;
    }

    public void SetDefenseTime(float timeInSecs)
    {
        defenseTime = timeInSecs;
        currentTime = 0.0f;
    }

    void IState.Update()
    {
        // do defense stuff
        foreach (Group group in controller.enemyGroups)
        {
            group.SetGroupDestination(defenseLocation.position, null);
        }

        // Check for state transition (defense -> harvest)
        currentTime += Time.deltaTime;
        if (currentTime > defenseTime)
        {
            currentTime = 0.0f;
            Debug.Log("Defense -> Harvest State");
            controller.State = EnemyController.harvestState;
        }

        // Check for state transition (defense -> offense)
        foreach (Group group in controller.enemyGroups)
        {
            if (group.GetFirstUnit().IsInBattle) continue;

            // Fighting seek
            foreach (Group playerGroup in controller.playerController.groups)
            {
                float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < OffenseState.attackRangeSqr)
                {
                    Debug.Log("Defense -> Offense State");
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
                    Debug.Log("Defense -> Offense State");
                    controller.State = EnemyController.offenseState;
                }
            }
        }
    }

}
