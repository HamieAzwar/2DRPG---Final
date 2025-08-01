using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType; // Must implement IEnemy
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (state)
        {
            case State.Roaming:
                Roaming(distanceToPlayer);
                break;

            case State.Attacking:
                Attacking(distanceToPlayer);
                break;
        }
    }

    private void Roaming(float distanceToPlayer)
    {
        timeRoaming += Time.deltaTime;
        enemyPathfinding.MoveTo(roamPosition);

        if (distanceToPlayer < attackRange)
        {
            state = State.Attacking;
            return;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking(float distanceToPlayer)
    {
        Vector2 dirToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;

        if (distanceToPlayer > attackRange)
        {
            state = State.Roaming;
            return;
        }

        if (stopMovingWhileAttacking)
        {
            enemyPathfinding.StopMoving();
        }
        else
        {
            enemyPathfinding.MoveTo(dirToPlayer);
        }

        if (canAttack)
        {
            canAttack = false;
            Debug.Log("Enemy is attacking!");

            if (enemyType is IEnemy enemy)
            {
                enemy.Attack();
            }
            else
            {
                Debug.LogWarning("enemyType doesn't implement IEnemy.");
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
