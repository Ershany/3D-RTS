using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestState : IState {

    private float wanderTime, currentTime;

    public HarvestState()
    {
        currentTime = wanderTime = 10.0f;
    }

    void IState.Update(EnemyController controller)
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


        // Check for state transition
        foreach (Group group in controller.enemyGroups)
        {
            if (group.GetFirstUnit().IsInBattle) continue;

            foreach (Group playerGroup in controller.playerController.groups)
            {
                float distanceSqr = (playerGroup.GetFirstUnit().GetTransform().position - group.GetFirstUnit().GetTransform().position).sqrMagnitude;
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
