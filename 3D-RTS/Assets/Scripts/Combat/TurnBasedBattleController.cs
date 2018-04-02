using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedBattleController
{
    public Group playerGroup;
    public Group enemyGroup;
    public List<DynamicUnit> randomBattleEnemies;
    //public List<Unit> combatants;
    public GameObject arena;
    public bool battleOver;

    private Queue<DynamicUnit> attackQueue;
    private DynamicUnit attackingUnit;
    private Vector3 returnPos;
    private bool returningToStartingPosition;
    private float delayTime;
    private bool delaying;
    private float initialStoppingDistance;

    public bool factionBattle;


    public TurnBasedBattleController(Vector3 pos, Group p, Group e, GameObject a)
    {
        this.playerGroup = p;
        this.enemyGroup = e;
        battleOver = false;
        attackQueue = new Queue<DynamicUnit>();
        returningToStartingPosition = false;
        delayTime = 0.0f;
        arena = a;
        delaying = false;
        initialStoppingDistance = playerGroup.GetUnits()[0].GetAgent().stoppingDistance;
        factionBattle = true;

        BoxCollider localCollider = arena.GetComponent<BoxCollider>();

        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(1, 1, 1);
            playerGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x + 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(playerGroup.GetUnits().Count + 1)) * (i + 1));
            Physics.IgnoreCollision(localCollider, playerGroup.GetUnits()[i].GetGameObject().GetComponent<Collider>());

            playerGroup.GetUnits()[i].GetTransform().LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));



            playerGroup.GetUnits()[i].attackTime = Mathf.Min((float)playerGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)playerGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            playerGroup.GetUnits()[i].GetAgent().stoppingDistance = 2.0f;
            playerGroup.GetUnits()[i].Halt();

        }

        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            enemyGroup.GetUnits()[i].GetTransform().localScale = new Vector3(1, 1, 1);
            enemyGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x - 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(enemyGroup.GetUnits().Count + 1)) * (i + 1));
            Physics.IgnoreCollision(localCollider, enemyGroup.GetUnits()[i].GetGameObject().GetComponent<Collider>());

            enemyGroup.GetUnits()[i].GetTransform().LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));


            enemyGroup.GetUnits()[i].attackTime = Mathf.Min((float)enemyGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)enemyGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            enemyGroup.GetUnits()[i].GetAgent().stoppingDistance = 2.0f;
            enemyGroup.GetUnits()[i].Halt();
        }


        enemyGroup.GetUnits()[0].GetTransform().LookAt(playerGroup.GetUnits()[0].GetTransform().position);
    }


    public TurnBasedBattleController(Vector3 pos, Group p, List<DynamicUnit> e, GameObject a)
    {
        this.playerGroup = p;
        this.randomBattleEnemies = e;
        battleOver = false;
        attackQueue = new Queue<DynamicUnit>();
        returningToStartingPosition = false;
        delayTime = 0.0f;
        arena = a;
        delaying = false;
        initialStoppingDistance = playerGroup.GetUnits()[0].GetAgent().stoppingDistance;
        factionBattle = false;

        BoxCollider localCollider = arena.GetComponent<BoxCollider>();

        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(1, 1, 1);
            playerGroup.GetUnits()[i].GetTransform().position = new Vector3(pos.x + 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(playerGroup.GetUnits().Count + 1)) * (i + 1));
            Physics.IgnoreCollision(localCollider, playerGroup.GetUnits()[i].GetGameObject().GetComponent<Collider>());

            playerGroup.GetUnits()[i].GetTransform().LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));



            playerGroup.GetUnits()[i].attackTime = Mathf.Min((float)playerGroup.GetUnits()[i].GetDexterity() * (1.0f - ((float)playerGroup.GetUnits()[i].GetDexterity() / 10f)) * 3.0f, 75.0f) / 100.0f;

            playerGroup.GetUnits()[i].GetAgent().stoppingDistance = 2.0f;
            playerGroup.GetUnits()[i].Halt();

        }

        for (int i = 0; i < randomBattleEnemies.Count; i++)
        {
            Debug.Log(randomBattleEnemies.Count);
            Debug.Log(randomBattleEnemies[i].GetTransform());
            randomBattleEnemies[i].GetTransform().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            randomBattleEnemies[i].GetTransform().position = new Vector3(pos.x - 0.45f * arena.transform.localScale.x, pos.y, pos.z - 0.5f * arena.transform.localScale.z + arena.transform.localScale.z * (1.0f / (float)(randomBattleEnemies.Count + 1)) * (i + 1));
            Physics.IgnoreCollision(localCollider, randomBattleEnemies[i].GetGameObject().GetComponent<Collider>());

            randomBattleEnemies[i].GetTransform().transform.LookAt(new Vector3(pos.x, playerGroup.GetUnits()[i].GetTransform().position.y, playerGroup.GetUnits()[i].GetTransform().position.z));


            randomBattleEnemies[i].attackTime = 0.5f + (float) Random.Range(0.0f, 1.0f);

            randomBattleEnemies[i].GetAgent().stoppingDistance = 2.0f;
            randomBattleEnemies[i].GetAgent().SetDestination(randomBattleEnemies[i].GetGameObject().transform.position);
        }


    }



    public void Update()
    {
        if (factionBattle)
        {
            UpdateFactionBattle();
        }
        else
        {
            UpdateRandomBattle();
        }

    }

    public void UpdateRandomBattle()
    {


        float deltaTime = Time.deltaTime;
        delayTime += deltaTime;

        int deathCount = 0;

        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].attackTime += deltaTime * ((float)playerGroup.GetUnits()[i].GetDexterity() / 25.0f) + deltaTime;
            if (playerGroup.GetUnits()[i].IsDead)
                deathCount++;
            if (playerGroup.GetUnits()[i].attackTime >= 5.0f && playerGroup.GetUnits()[i].IsDead == false)
                if (!attackQueue.Contains(playerGroup.GetUnits()[i]) && attackingUnit != playerGroup.GetUnits()[i])
                {
                    attackQueue.Enqueue(playerGroup.GetUnits()[i]);
                    playerGroup.GetUnits()[i].BeginAttack(randomBattleEnemies);
                }
        }


        if (deathCount >= playerGroup.GetUnits().Count)
        {
            battleOver = true;
        }
        else
            deathCount = 0;
        for (int i = 0; i < randomBattleEnemies.Count; i++)
        {
            if (randomBattleEnemies[i].IsDead)
                deathCount++;
            else
                randomBattleEnemies[i].attackTime += deltaTime * (float)(randomBattleEnemies[i].GetDexterity() / 25) + deltaTime;

            if (randomBattleEnemies[i].attackTime >= 5.0f && randomBattleEnemies[i].IsDead == false)
                if (!attackQueue.Contains(randomBattleEnemies[i]) && attackingUnit != randomBattleEnemies[i])
                {
                    attackQueue.Enqueue(randomBattleEnemies[i]);
                    randomBattleEnemies[i].BeginAttack(playerGroup.GetUnits());
                }
        }

        if (deathCount >= randomBattleEnemies.Count)
        {
            battleOver = true;
        }
        if (attackQueue.Count > 0)
        {

            if (attackingUnit == null)
            {
                attackingUnit = attackQueue.Dequeue();
                returnPos = attackingUnit.GetTransform().position;
                attackingUnit.AttackAnyEnemy();
            }

            else if (ResolvedAttack() || attackingUnit.IsDead)
            {
                attackingUnit = attackQueue.Dequeue();
                returnPos = attackingUnit.GetTransform().position;
                attackingUnit.AttackAnyEnemy();
            }

        }


        if (attackingUnit != null && !ResolvedAttack())
        {

            if (!attackingUnit.IsAttacking && !delaying)
            {
                Debug.Log("Check1");
                if (!attackingUnit.GetAgent().pathPending && attackingUnit.GetAgent().remainingDistance <= attackingUnit.GetAgent().stoppingDistance)
                {
                    Debug.Log("Check2");
                    if (!returningToStartingPosition)
                    {
                        Debug.Log("Check3");
                        returningToStartingPosition = true;

                        attackingUnit.GetAgent().stoppingDistance = 0.1f;
                        attackingUnit.SetDestination(returnPos);
                        Debug.Log("Returning to " + returnPos.ToString());
                    }
                    else if (attackingUnit.GetTransform().position != returnPos)
                    {
                        attackingUnit.GetTransform().position = returnPos;
                    }
                    else
                    {
                        attackingUnit.GetAgent().stoppingDistance = 2.0f;
                        returningToStartingPosition = false;
                        attackingUnit.GetTransform().LookAt(new Vector3(arena.transform.position.x, attackingUnit.GetTransform().position.y, attackingUnit.GetTransform().position.z));
                        attackingUnit.attackTime = 0.0f;
                        delaying = true;
                        delayTime = 0;
                    }
                }

            }
        }

        if (battleOver)
        {
            for (int i = 0; i < playerGroup.GetUnits().Count; i++)
            {
                playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(4, 4, 4);
                playerGroup.GetUnits()[i].GetAgent().stoppingDistance = initialStoppingDistance;
            }
            for (int i = 0; i < randomBattleEnemies.Count; i++)
            {
                playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(4, 4, 4);
                randomBattleEnemies[i].GetAgent().stoppingDistance = initialStoppingDistance;
            }
        }
    }


    public void UpdateFactionBattle()
    {

        float deltaTime = Time.deltaTime;
        delayTime += deltaTime;

        int deathCount = 0;

        for (int i = 0; i < playerGroup.GetUnits().Count; i++)
        {
            playerGroup.GetUnits()[i].attackTime += deltaTime * ((float)playerGroup.GetUnits()[i].GetDexterity() / 25.0f) + deltaTime;
            if (playerGroup.GetUnits()[i].IsDead)
                deathCount++;
            if (playerGroup.GetUnits()[i].attackTime >= 5.0f && playerGroup.GetUnits()[i].IsDead == false)
                if (!attackQueue.Contains(playerGroup.GetUnits()[i]) && attackingUnit != playerGroup.GetUnits()[i])
                {
                    attackQueue.Enqueue(playerGroup.GetUnits()[i]);
                    playerGroup.GetUnits()[i].BeginAttack(enemyGroup.GetUnits());
                }
        }


        if (deathCount >= playerGroup.GetUnits().Count)
        {
            battleOver = true;
        }
        else
            deathCount = 0;
        for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
        {
            if (enemyGroup.GetUnits()[i].IsDead)
                deathCount++;
            else
                enemyGroup.GetUnits()[i].attackTime += deltaTime * (float)(enemyGroup.GetUnits()[i].GetDexterity() / 25) + deltaTime;

            if (enemyGroup.GetUnits()[i].attackTime >= 5.0f && enemyGroup.GetUnits()[i].IsDead == false)
                if (!attackQueue.Contains(enemyGroup.GetUnits()[i]) && attackingUnit != enemyGroup.GetUnits()[i])
                {
                    attackQueue.Enqueue(enemyGroup.GetUnits()[i]);
                    enemyGroup.GetUnits()[i].BeginAttack(playerGroup.GetUnits());
                }
        }

        if (deathCount >= enemyGroup.GetUnits().Count)
        {
            battleOver = true;
        }
        if (attackQueue.Count > 0)
        {

            if (attackingUnit == null)
            {
                attackingUnit = attackQueue.Dequeue();
                returnPos = attackingUnit.GetTransform().position;
                attackingUnit.AttackAnyEnemy();
            }

            else if (ResolvedAttack() || attackingUnit.IsDead)
            {
                attackingUnit = attackQueue.Dequeue();
                returnPos = attackingUnit.GetTransform().position;
                attackingUnit.AttackAnyEnemy();
            }

        }


        if (attackingUnit != null && !ResolvedAttack())
        {

            if (!attackingUnit.IsAttacking && !delaying)
            {
                Debug.Log("Check1");
                if (!attackingUnit.GetAgent().pathPending && attackingUnit.GetAgent().remainingDistance <= attackingUnit.GetAgent().stoppingDistance)
                {
                    Debug.Log("Check2");
                    if (!returningToStartingPosition)
                    {
                        Debug.Log("Check3");
                        returningToStartingPosition = true;

                        attackingUnit.GetAgent().stoppingDistance = 0.1f;
                        attackingUnit.SetDestination(returnPos);
                        Debug.Log("Returning to " + returnPos.ToString());
                    }
                    else if (attackingUnit.GetTransform().position != returnPos)
                    {
                        attackingUnit.GetTransform().position = returnPos;
                    }
                    else
                    {
                        attackingUnit.GetAgent().stoppingDistance = 2.0f;
                        returningToStartingPosition = false;
                        attackingUnit.GetTransform().LookAt(new Vector3(arena.transform.position.x, attackingUnit.GetTransform().position.y, attackingUnit.GetTransform().position.z));
                        attackingUnit.attackTime = 0.0f;
                        delaying = true;
                        delayTime = 0;
                    }
                }

            }
        }

        if (battleOver)
        {
            for (int i = 0; i < playerGroup.GetUnits().Count; i++)
            {
                playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(4, 4, 4);
                playerGroup.GetUnits()[i].GetAgent().stoppingDistance = initialStoppingDistance;
            }
            for (int i = 0; i < enemyGroup.GetUnits().Count; i++)
            {
                playerGroup.GetUnits()[i].GetTransform().localScale = new Vector3(4, 4, 4);
                enemyGroup.GetUnits()[i].GetAgent().stoppingDistance = initialStoppingDistance;
            }
        }
    }


    public bool ResolvedAttack()
    {

        if (delaying && delayTime > 1.0)
        {
            delaying = false;
            return true;
        }

        return false;
    }


}
