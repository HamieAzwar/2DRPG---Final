using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    public void Attack()
    {
        Debug.Log("SwordEnemy attempting to attack...");

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
        if (hit != null && hit.TryGetComponent(out PlayerHealth playerHealth))
        {
            Debug.Log("SwordEnemy hit player!");
            playerHealth.TakeDamage(damage, attackPoint);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform usedAttackPoint = attackPoint != null ? attackPoint : transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(usedAttackPoint.position, attackRadius);
    }
}
