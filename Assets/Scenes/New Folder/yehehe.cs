using UnityEngine;
using UnityEngine.InputSystem;

public class MultiAnimatorTrigger : MonoBehaviour
{
    [Header("Animators")]
    public Animator clownAnimator;
    public Animator yohohoAnimator;
    public Animator yihihiAnimator;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (clownAnimator != null) clownAnimator.SetTrigger("Clown");
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (yohohoAnimator != null) yohohoAnimator.SetTrigger("Yohoho");
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (yihihiAnimator != null) yihihiAnimator.SetTrigger("Yihihi");
        }
    }
}