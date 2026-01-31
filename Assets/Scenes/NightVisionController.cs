using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // WAJIB: Tambahkan namespace ini

public class NightVisionController : MonoBehaviour
{
    // Kita tidak butuh variabel Material lagi karena kita main hide/show object
    // public Material nightVisionMaterial; 

    // List untuk menyimpan referensi GameObject itu sendiri
    private List<GameObject> hiddenObjects = new List<GameObject>();

    private bool isNightVisionActive = false;

    void Start()
    {
        // Cari semua objek dengan tag "HiddenObject"
        GameObject[] targets = GameObject.FindGameObjectsWithTag("HiddenObject");

        foreach (GameObject go in targets)
        {
            // Masukkan ke dalam list
            hiddenObjects.Add(go);

            // PENTING: Matikan (hide) objek saat game baru mulai
            // Jadi default-nya tidak terlihat
            go.SetActive(false);
        }
    }

    void Update()
    {
        // Cek apakah keyboard ada dan tombol N ditekan (sesuai request)
        // Sebelumnya script Anda menggunakan 'qKey', sekarang diganti 'nKey'
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            ToggleNightVision();
        }
    }

    void ToggleNightVision()
    {
        // Ubah status true/false
        isNightVisionActive = !isNightVisionActive;

        // Loop semua objek yang sudah didaftarkan
        foreach (GameObject go in hiddenObjects)
        {
            if (go != null)
            {
                // SetActive true = Muncul
                // SetActive false = Hilang
                go.SetActive(isNightVisionActive);
            }
        }
    }
}