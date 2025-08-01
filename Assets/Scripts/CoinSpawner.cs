using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int numberOfCoins = 10;
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-5, -5);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(5, 5);

    private void Start()
    {
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
