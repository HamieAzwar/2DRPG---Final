using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead {get; private set;}

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider; 
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Scene1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    /// <summary>
    /// Used if you want enemies to deal damage by staying in contact with the player.
    /// Can be disabled if you're using attack hitboxes (e.g., SwordEnemy).
    /// </summary>
    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) return;

        Debug.Log($"Player took {damageAmount} damage from {hitTransform.name}");

        if (knockback != null)
        {
            knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        canTakeDamage = false;

        StartCoroutine(DamageRecoveryRoutine());

        Debug.Log("Current Player Health: " + currentHealth);

        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f); // Wait for death animation to finish
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }
        
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        // Optional: Trigger healing animation, sound, or UI update
    }

    public bool IsAtMaxHealth()
    {
        return currentHealth >= maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // ✅ Add this method for the HealthUI to work
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
