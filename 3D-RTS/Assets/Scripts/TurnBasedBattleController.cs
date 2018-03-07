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

    public TurnBasedBattleController(Vector3 pos, Group p, Group e, GameObject a)
    {
        this.playerGroup = p;
        this.enemyGroup = e;
        attackQueue = new Queue<DynamicUnit>();
        rpSet = false;

        arena = a;
        

        for(int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x + 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(playerGroup.GetUnits().Count + 1)) * (i + 1));


            //playerGroup.GetUnits()[i].GetTransform().rotation = new Quaternion(0.0f, -90.0f, 0.0f, 1.0f);
            //playerGroup.GetUnits()[i].GetTransform().Rotate(0,90,0);

            playerGroup.GetUnits()[i].attackTime = Mathf.Min((float)playerGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)playerGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            playerGroup.GetUnits()[i].Halt();

        }

        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            enemyGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x - 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(playerGroup.GetUnits().Count + 1)) * (i + 1));

            //enemyGroup.GetUnits()[i].GetTransform().rotation = new Quaternion(0.0f, 90.0f, 0.0f, 1.0f);
           // enemyGroup.GetUnits()[i].GetTransform().Rotate(0, -90, 0);

            enemyGroup.GetUnits()[i].attackTime = Mathf.Min((float)playerGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)playerGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            enemyGroup.GetUnits()[i].Halt();
        }


        playerGroup.GetUnits()[0].GetTransform().LookAt(enemyGroup.GetUnits()[0].GetTransform().position);
        enemyGroup.GetUnits()[0].GetTransform().LookAt(playerGroup.GetUnits()[0].GetTransform().position);
    }



    public void Update () {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].attackTime += deltaTime * ((float)playerGroup.GetUnits()[i].GetDexterity() / 25.0f) + deltaTime;

            if (playerGroup.GetUnits()[i].attackTime >= 1)
                if (!attackQueue.Contains(playerGroup.GetUnits()[i]))
                {
                    attackQueue.Enqueue(playerGroup.GetUnits()[i]);
                    playerGroup.GetUnits()[i].BeginAttack(enemyGroup.GetUnits());
                }
        }

        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            enemyGroup.GetUnits()[i].attackTime += deltaTime * (float)(playerGroup.GetUnits()[i].GetDexterity() / 25) + deltaTime;

            if (enemyGroup.GetUnits()[i].attackTime >= 1.0f)
                if (!attackQueue.Contains(enemyGroup.GetUnits()[i]))
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
                    attackingUnit.SetDestination(returnPos);
                    Debug.Log("Returning to " + returnPos.ToString());
;                    rpSet = true;
                }
                if (attackQueue.Count > 0 && attackingUnit.GetTransform().position == returnPos)
                {
                    rpSet = false;
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
