using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoins(coinValue);

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
