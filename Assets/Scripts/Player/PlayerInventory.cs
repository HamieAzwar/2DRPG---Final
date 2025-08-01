using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private int coins = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins: " + coins);
    }
}
