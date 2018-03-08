using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedBattleController {


    public Group playerGroup;
    public Group enemyGroup;
    //public List<Unit> combatants;
    public GameObject arena;

    private Queue<DynamicUnit> attackQueue;
    private DynamicUnit attackingUnit;
    private Vector3 returnPos;
    private bool rpSet;
    private float delayTime;
    private bool delaying;

    public TurnBasedBattleController(Vector3 pos, Group p, Group e, GameObject a)
    {
        this.playerGroup = p;
        this.enemyGroup = e;
        attackQueue = new Queue<DynamicUnit>();
        rpSet = false;
        delayTime = 0.0f;
        arena = a;
        delaying = false;
        

        for(int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x + 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(playerGroup.GetUnits().Count + 1)) * (i + 1));


            playerGroup.GetUnits()[i].GetTransform().LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));

            playerGroup.GetUnits()[i].attackTime = Mathf.Min((float)playerGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)playerGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            playerGroup.GetUnits()[i].Halt();

        }

        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            enemyGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x - 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(enemyGroup.GetUnits().Count + 1)) * (i + 1));

            enemyGroup.GetUnits()[i].GetTransform().LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));


            enemyGroup.GetUnits()[i].attackTime = Mathf.Min((float)enemyGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)enemyGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            enemyGroup.GetUnits()[i].Halt();
        }


        enemyGroup.GetUnits()[0].GetTransform().LookAt(playerGroup.GetUnits()[0].GetTransform().position);
    }



    public void Update () {
        float deltaTime = Time.deltaTime;
        delayTime += deltaTime;
        Debug.Log("Delay Time: " + delayTime.ToString());
        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].attackTime += deltaTime * ((float)playerGroup.GetUnits()[i].GetDexterity() / 25.0f) + deltaTime;

            if (playerGroup.GetUnits()[i].attackTime >= 3.0f)
                if (!attackQueue.Contains(playerGroup.GetUnits()[i]) && attackingUnit != playerGroup.GetUnits()[i])
                {
                    attackQueue.Enqueue(playerGroup.GetUnits()[i]);
                    playerGroup.GetUnits()[i].BeginAttack(enemyGroup.GetUnits());
                }
        }

        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            enemyGroup.GetUnits()[i].attackTime += deltaTime * (float)(enemyGroup.GetUnits()[i].GetDexterity() / 25) + deltaTime;

            if (enemyGroup.GetUnits()[i].attackTime >= 3.0f)
                if (!attackQueue.Contains(enemyGroup.GetUnits()[i]) && attackingUnit != enemyGroup.GetUnits()[i])
                {
                    attackQueue.Enqueue(enemyGroup.GetUnits()[i]);
                    enemyGroup.GetUnits()[i].BeginAttack(playerGroup.GetUnits());
                }
        }

        if(attackingUnit != null)
        {
            if (!attackingUnit.IsAttacking)
            {
                if (!rpSet)
                {

                    returnPos = returnPos + Vector3.Normalize(returnPos - attackingUnit.GetTransform().position) * 1.5f;

                    attackingUnit.SetDestination(returnPos);
                    Debug.Log("Returning to " + returnPos.ToString());
;                    rpSet = true;
                }
                if (!attackingUnit.GetAgent().pathPending && attackingUnit.GetAgent().remainingDistance <= attackingUnit.GetAgent().stoppingDistance && !delaying)
                {
                    rpSet = false;
                    attackingUnit.GetTransform().LookAt(new Vector3(arena.transform.position.x, attackingUnit.GetTransform().position.y, attackingUnit.GetTransform().position.z));
                    attackingUnit.attackTime = 0.0f;
                    delaying = true;
                    delayTime = 0;
                }
                if (attackQueue.Count > 0 && delayTime > 1.0f && delaying)
                {
                    delaying = false;
                    attackingUnit = attackQueue.Dequeue();
                    returnPos = attackingUnit.GetTransform().position;
                    attackingUnit.AttackAnyEnemy();
                }

            }
        }
        else if (attackQueue.Count > 0)
        {
            rpSet = false;
            attackingUnit = attackQueue.Dequeue();
            returnPos = attackingUnit.GetTransform().position;
            attackingUnit.AttackAnyEnemy();
        }
    }

}
