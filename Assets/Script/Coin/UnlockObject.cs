using UnityEngine;

public class UnlockObject : MonoBehaviour
{
    public int requiredCoin = 5;
    public GameObject objectToUnlock;

    public void CheckUnlock(int currentCoin)
    {
        if (currentCoin >= requiredCoin)
        {
            objectToUnlock.SetActive(true);
        }
    }
}
