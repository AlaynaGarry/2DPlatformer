using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Health playerHealth;
    [SerializeField] Image totalHealthBar;
    [SerializeField] Image currentHealthBar;
    void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 5;
    }

    void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 5;
    }
}
