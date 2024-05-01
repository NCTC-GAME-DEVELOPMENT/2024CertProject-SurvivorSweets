using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAttack : MonoBehaviour, IAttack {
    [Header("Main Statistics")]
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float attackCooldown;
    [Header("Debug")]
    [SerializeField]
    private bool isCloseEnoughToAttack;
    public void UpdateDistanceToTarget(float distance) {
        if (distance < attackRange && !isCloseEnoughToAttack) {
            isCloseEnoughToAttack = true;
            StartCoroutine(AttackCycle());
        }
        else if (distance > attackRange && isCloseEnoughToAttack)
            isCloseEnoughToAttack = false;
    
    }
    public void Start() {
        if (attackCooldown < 0.25f) {
            attackCooldown = 0.25f;
        }
    }
    IEnumerator AttackCycle() {
        while (isCloseEnoughToAttack) {
            Attack();
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void Attack() {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hit in hits) {
            if (hit.tag == "Player") {
                var health = hit.GetComponent<IHealth>();
                if (health is null)
                    throw new System.Exception("Player does not have an \"IHealth\" Component!");
                health.DoDamage(attackDamage);
            }
        }
    }
}
