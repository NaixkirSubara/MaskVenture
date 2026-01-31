using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Setting")]
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager coinManager = other.GetComponent<CoinManager>();
            if (coinManager != null)
            {
                coinManager.AddCoin(coinValue);
            }

            Destroy(gameObject);
        }
    }
}
