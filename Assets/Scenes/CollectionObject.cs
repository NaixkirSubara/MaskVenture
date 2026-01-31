using UnityEngine;
using System.Collections; // Butuh ini untuk Coroutine

public class CollectionObject : MonoBehaviour
{
    [Header("--- Konfigurasi ---")]
    public GameObject[] requiredItems;
    public GameObject objectToActivate;

    [Header("--- Tombol & Waktu ---")]
    public KeyCode actionButton = KeyCode.E;
    public float interactionDelay = 2.0f; // Waktu "casting" saat mengaktifkan target

    private int collectedCount = 0;

    void Start()
    {
        if (objectToActivate != null) objectToActivate.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < requiredItems.Length; i++)
        {
            if (requiredItems[i] != null && other.gameObject == requiredItems[i])
            {
                CollectItem(i);
                break;
            }
        }
    }

    void CollectItem(int index)
    {
        Destroy(requiredItems[index]);
        collectedCount++;
        Debug.Log("Item Hancur! Total: " + collectedCount + " / " + requiredItems.Length);

        if (collectedCount >= requiredItems.Length)
        {
            Debug.Log("SIAP! Tekan " + actionButton + " untuk mengaktifkan objek target.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(actionButton))
        {
            TryActivateTarget();
        }
    }

    void TryActivateTarget()
    {
        // 1. Cek jumlah item
        if (collectedCount < requiredItems.Length)
        {
            int sisa = requiredItems.Length - collectedCount;
            Debug.Log("GAGAL: Masih kurang " + sisa + " item lagi.");
            return;
        }

        // 2. INTEGRASI: Cek Manager (Apakah skill lain sedang aktif?)
        if (GlobalAbilityManager.Instance.TryActivateAbility())
        {
            // Jika berhasil, mulai proses aktivasi (gunakan Coroutine untuk durasi)
            StartCoroutine(ActivationProcess());
        }
    }

    // Proses aktivasi dengan durasi agar tombol lain terkunci sementara
    IEnumerator ActivationProcess()
    {
        Debug.Log("Sedang mengaktifkan mekanisme... (Sistem Terkunci)");

        // Tunggu selama interactionDelay (misal: animasi buka pintu)
        yield return new WaitForSeconds(interactionDelay);

        if (objectToActivate != null && !objectToActivate.activeSelf)
        {
            objectToActivate.SetActive(true);
            Debug.Log("SUKSES: Objek Target Telah Diaktifkan!");
        }

        // INTEGRASI: Lepas kunci setelah proses selesai
        GlobalAbilityManager.Instance.FinishAbility();
    }
}