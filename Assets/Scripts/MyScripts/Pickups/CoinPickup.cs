using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //add change to UI element
            GameManager.Instance.Score += 1;
            SoundManager.instance.PlaySound(coinSound);
            gameObject.SetActive(false);
        }
    }
}
