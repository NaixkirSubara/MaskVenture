using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapRoulette : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform contentContainer; // Slot "ContentContainer" (Objek yang punya Horizontal Layout Group)
    public GameObject mapCardPrefab;       // Prefab kartu map (Harus punya component Image)

    [Header("Data")]
    public Sprite[] mapThumbnails;         // Daftar Gambar Map
    public string[] sceneNames;            // Daftar Nama Scene (Harus sama urutannya dengan gambar)

    [Header("Settings")]
    public int totalItemsToSpawn = 40;     // Total kartu yang akan digenerate
    public float spinDuration = 3.0f;      // Lama putaran
    public AnimationCurve speedCurve;      // Kurva kecepatan (Slow in - Fast - Slow out)
    public float cardWidth = 200f;         // Lebar satu kartu (Pixel)
    public float spacing = 20f;            // Jarak antar kartu (Pixel)

    private bool isSpinning = false;

    void Start()
    {
        // --- PERBAIKAN UTAMA DI SINI ---
        // Kita cek dulu apakah speedCurve 'null' (belum diassign)
        if (speedCurve == null)
        {
            speedCurve = new AnimationCurve();
        }

        // Jika kurva masih kosong (length 0), kita buat defaultnya
        if (speedCurve.length == 0)
        {
            speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }

    // Panggil fungsi ini dari Button UI atau OnMouseDown
    public void StartSpin()
    {
        if (isSpinning) return;
        
        // Cek keamanan sebelum mulai
        if (contentContainer == null || mapCardPrefab == null)
        {
            Debug.LogError("UI References belum diisi di Inspector!");
            return;
        }

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;

        // 1. BERSIHKAN CONTAINER LAMA (Hapus anak-anak object sebelumnya)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. TENTUKAN PEMENANG (Kecuali scene saat ini)
        string currentScene = SceneManager.GetActiveScene().name;
        List<int> validIndices = new List<int>();

        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (sceneNames[i] != currentScene) 
            {
                validIndices.Add(i);
            }
        }

        // Jika tidak ada scene lain, stop agar tidak error
        if (validIndices.Count == 0) 
        { 
            Debug.LogError("Tidak ada scene lain di Build Settings untuk dipilih!"); 
            isSpinning = false;
            yield break; 
        }
        
        // Ambil index asli dari scene terpilih
        int winningIndex = validIndices[Random.Range(0, validIndices.Count)];
        Sprite winnerSprite = mapThumbnails[winningIndex];

        // 3. GENERATE STRIP GAMBAR (Menyusun kartu memanjang)
        // Kita taruh pemenang agak di akhir (misal: total - 5)
        int targetPositionIndex = totalItemsToSpawn - 5; 

        for (int i = 0; i < totalItemsToSpawn; i++)
        {
            // Spawn kartu baru
            GameObject newCard = Instantiate(mapCardPrefab, contentContainer);
            
            // Ambil component Image pada prefab tersebut
            Image img = newCard.GetComponent<Image>(); 
            
            // Safety check jika prefab tidak punya Image
            if (img == null) img = newCard.GetComponentInChildren<Image>();

            if (i == targetPositionIndex)
            {
                // INI KARTU PEMENANG
                img.sprite = winnerSprite;
                newCard.name = "WINNER CARD";
            }
            else
            {
                // INI KARTU PENGECOH (Random visual saja)
                img.sprite = mapThumbnails[Random.Range(0, mapThumbnails.Length)];
                newCard.name = "Dummy Card";
            }
        }

        // Tunggu 1 frame agar Unity selesai menata Layout Group
        yield return null; 

        // 4. HITUNG POSISI TARGET
        // Rumus: -(Index * (Lebar + Spasi))
        // Ditambah setengah lebar layar (opsional) agar pas di tengah, tapi rumus dasar ini biasanya cukup
        float finalX = -1f * (targetPositionIndex * (cardWidth + spacing));

        // 5. ANIMASI GERAK (LERP)
        float timer = 0f;
        Vector2 startPos = contentContainer.anchoredPosition;
        
        // Kita hanya menggeser sumbu X, sumbu Y tetap
        Vector2 endPos = new Vector2(finalX, startPos.y);

        while (timer < spinDuration)
        {
            timer += Time.deltaTime;
            float percentage = timer / spinDuration;
            
            // Evaluasi posisi berdasarkan kurva
            float curveValue = speedCurve.Evaluate(percentage); 

            contentContainer.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            yield return null;
        }

        // Pastikan posisi akhir tepat di target
        contentContainer.anchoredPosition = endPos;
        
        // Jeda sedikit untuk dramatisir sebelum pindah scene
        yield return new WaitForSeconds(1f); 
        
        // 6. PINDAH SCENE
        SceneManager.LoadScene(sceneNames[winningIndex]);
    }
}