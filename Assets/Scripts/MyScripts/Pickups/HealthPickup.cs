using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float healthValue;
    [SerializeField] AudioClip healthSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.GetComponent<Health>().AddHealth(healthValue);
            SoundManager.instance.PlaySound(healthSound);
            gameObject.SetActive(false);
        }
    }
}
