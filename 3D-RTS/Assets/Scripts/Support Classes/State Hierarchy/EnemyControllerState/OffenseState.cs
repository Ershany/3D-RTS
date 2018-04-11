using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenseState : IState {

    private EnemyController controller;
    private PlayerController playerController;
    private Transform guildhallLocation;

    public static float attackRangeSqr = 55.0f * 55.0f;
    
    public OffenseState(GameObject controller)
    {
        this.controller = controller.GetComponent<EnemyController>();
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

        guildhallLocation = null;
    }

    void IState.Update()
    {
        foreach (Group group in controller.enemyGroups)
        {
            if (group.GetFirstUnit().IsInBattle) continue;

            Transform positionToAttack = null;
            float lowestDistance = float.MaxValue;
            foreach (Group playerGroup in controller.playerController.groups)
            {
                float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < attackRangeSqr && distanceSqr < lowestDistance)
                {
                    positionToAttack = playerGroup.GetFirstUnit().GetTransform();
                    lowestDistance = distanceSqr;
                }
            }
            if (playerController.guildHallBuilt && guildhallLocation == null)
            {
                guildhallLocation = GameObject.FindGameObjectWithTag("GuildHall").transform;
            }
            if (guildhallLocation != null)
            {
                float distanceSqr = (guildhallLocation.position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < attackRangeSqr && distanceSqr < lowestDistance)
                {
                    positionToAttack = guildhallLocation;
                    lowestDistance = distanceSqr;
                }
            }
            

            if (positionToAttack != null)
                group.SetGroupDestination(positionToAttack.position, null);
        }
    }
}
