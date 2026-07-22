using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;


public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardViews> cards = new(); //list drawed card on hand
    public IEnumerator AddCard(CardViews cardView)
    {
        cards.Add(cardView);// Masukkan kartu baru ke daftar.
        yield return UpdateCardPosition(0.15f);// Jalankan fungsi penataan posisi kartu.
    }
    private IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0) yield break;

        // cardSpacing: Jarak antar kartu. Semakin banyak kartu, jarak ini bisa dikecilkan.
        float cardSpacing = 1f / 10f;
        // Titik 0.5f adalah tepat di TENGAH garis Spline. 
        // Rumus ini memastikan susunan kartu selalu berada di tengah lengkungan.
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            // Menghitung posisi (p) untuk kartu ke-i di sepanjang garis lengkung.
            float p = firstCardPosition + i * cardSpacing;

            // Mengambil koordinat 3D dari titik lengkung tersebut.
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            // Menghitung rotasi agar kartu miring mengikuti lengkungan garis.
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            // --- Animasi DOTween ---
            // DOMove: Menggerakkan kartu ke posisi baru dengan tambahan sedikit sumbu Z (Vector3.back) 
            // agar kartu yang ditarik lebih akhir menumpuk di atas kartu sebelumnya.
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            // DORotate: Memutar kartu agar miring sesuai lengkungan.
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }
    // Fungsi ini dipanggil oleh kartu sesaat sebelum ia dihancurkan
    public void RemoveCard(CardViews cardToRemove)
    {
        // GANTI 'namaListKartumu' dengan nama variabel List yang ada di skrip HandView-mu!
        if (cards.Contains(cardToRemove))
        {
            cards.Remove(cardToRemove);
            
            // Hapus juga data yang null / kosong (tindakan pencegahan ekstra)
            cards.RemoveAll(item => item == null);
            
            // Tata ulang sisa kartu yang ada di tangan agar tidak bolong
            StartCoroutine(UpdateCardPosition(0.15f));
        }
    }
}
