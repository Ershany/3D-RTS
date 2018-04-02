using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenseState : IState {

    public static float attackRangeSqr = 55.0f * 55.0f;
    
    public OffenseState()
    {
        
    }

    void IState.Update(EnemyController controller)
    {
        foreach (Group group in controller.enemyGroups)
        {
            if (group.GetFirstUnit().IsInBattle) continue;

            Group groupToAttack = null;
            float lowestDistance = float.MaxValue;
            foreach (Group playerGroup in controller.playerController.groups)
            {
                float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
                if (distanceSqr < attackRangeSqr && distanceSqr < lowestDistance)
                {
                    groupToAttack = playerGroup;
                    lowestDistance = distanceSqr;
                }
            }

            if (groupToAttack != null)
                group.SetGroupDestination(groupToAttack.GetFirstUnit().GetTransform().position, null);
        }
    }
}
