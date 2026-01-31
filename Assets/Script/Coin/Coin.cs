using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Setting")]
    public int coinValue = 1;

    [Header("Audio")]
    public AudioClip coinSFX; // drag di Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CoinManager coinManager = other.GetComponent<CoinManager>();
        if (coinManager != null)
        {
            coinManager.AddCoin(coinValue);
        }

        //  Mainkan SFX coin
        if (coinSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(coinSFX);
        }

        Destroy(gameObject);
    }
}

