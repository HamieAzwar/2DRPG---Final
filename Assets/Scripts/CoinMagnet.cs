using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    public float attractRange = 3f;
    public float moveSpeed = 5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attractRange)
        {
            // Smooth move toward player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}
