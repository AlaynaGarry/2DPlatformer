using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float startingHealth;
    public float currentHealth { get; private set; }
    Animator animator;
    [HideInInspector]
    public bool dead;

    [Header("iFrames")]
    [SerializeField] float iFramesDuration;
    [SerializeField] int flashes;
    SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] Behaviour[] components;
    
    [Header("Death Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);*/
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
        dead = false;
        spriteRenderer.color = Color.white;
        foreach (Behaviour comp in components)
        {
            comp.enabled = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hurtSound);
            //iframes
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                animator.SetTrigger("die");

                foreach (Behaviour comp in components)
                {
                    comp.enabled = false;
                }

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
                GameManager.Instance.OnPlayerDead();
                Deactivate();
            }
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < flashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (flashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (flashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
