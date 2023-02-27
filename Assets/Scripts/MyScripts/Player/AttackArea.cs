using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>() != null && collision.tag != "Player") {
            Health health = collision.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }
}
