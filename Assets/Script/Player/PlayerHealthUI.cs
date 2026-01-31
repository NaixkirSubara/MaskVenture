using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Life Icons")]
    public Image[] lifeIcons;

    public void UpdateLife(int currentLife)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < currentLife;
        }
    }
}
