using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool triggerOnce;

    private void OnDisable()
    {
        CoinSpawner.Instance.coinPool.Release(gameObject);
        triggerOnce = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // todo fix later
        if (!triggerOnce && other.CompareTag("Player"))
        {
            triggerOnce = true;
            gameObject.SetActive(false);
        }
    }
}
