using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attack Param")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeX;
    [SerializeField] private float rangeY;
    [SerializeField] private int damage;

    [Header("Collider Param")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    float cooldoenTimer = Mathf.Infinity;

    Health playerHealth;

    Animator animator;
    EnemyPatrol enemyPatrol;

    float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldoenTimer += Time.deltaTime;
    
        if (PlayerInSight())
        {
            if (cooldoenTimer >= attackCooldown)
            {
                cooldoenTimer = 0;
                animator.SetTrigger("attack");
            }
        }
        if (enemyPatrol != null) {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    bool PlayerInSight()
    {
        timer += Time.deltaTime;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeX * rangeY * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeX, boxCollider.bounds.size.y * rangeY, boxCollider.bounds.size.z),
            0, Vector2.right, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeX * rangeY * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeX, boxCollider.bounds.size.y * rangeY, boxCollider.bounds.size.z));
    }

    void DamagePlayer()
    {
        if (PlayerInSight() && timer <= 1)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
