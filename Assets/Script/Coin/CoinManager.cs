using UnityEngine;
using UnityEngine.Events;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Data")]
    public int currentCoin;

    [Header("Event")]
    public UnityEvent<int> onCoinChanged;

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        Debug.Log("Coin sekarang: " + currentCoin);

        onCoinChanged?.Invoke(currentCoin);
    }
}
