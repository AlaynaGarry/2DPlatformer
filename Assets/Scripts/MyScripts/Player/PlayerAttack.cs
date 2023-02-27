using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCooldown;
    [SerializeField] AudioClip attackSound;

    [SerializeField] public Vector3 attackOffset;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public LayerMask attackMask;
    [SerializeField] public int damage;

    Animator animator;
    PlayerMovement playerMovement;
    //float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && /*cooldownTimer > attackCooldown &&*/ playerMovement.CanAttack())
            Attack();

        //cooldownTimer += Time.deltaTime;
    }

    void Attack() {
        animator.SetTrigger("attack");
        DealDamage();
        //cooldownTimer = 0;
    }

    public void DealDamage() {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
            colInfo.GetComponent<Health>().TakeDamage(damage * 5);

    }
}
