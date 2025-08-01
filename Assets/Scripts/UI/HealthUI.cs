using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private List<Image> hearts = new List<Image>();
    private int previousHealth = -1;

    void Start()
    {
        InitializeHearts();
        UpdateHearts();
    }

    void Update()
    {
        // Only update if health changed
        int currentHealth = playerHealth.GetCurrentHealth();
        if (currentHealth != previousHealth)
        {
            UpdateHearts();
            previousHealth = currentHealth;
        }
    }

    void InitializeHearts()
    {
        int maxHealth = playerHealth.GetMaxHealth(); // You need this method

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();
            hearts.Add(heartImage);
        }
    }

    void UpdateHearts()
    {
        int currentHealth = playerHealth.GetCurrentHealth();

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
    }
}
