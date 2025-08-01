using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private int currentCoins = 0;

    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log("Coins collected: " + currentCoins); // For debugging
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + currentCoins;
        }
        else
        {
            Debug.LogWarning("CoinManager: coinText is not assigned!");
        }
    }
}
