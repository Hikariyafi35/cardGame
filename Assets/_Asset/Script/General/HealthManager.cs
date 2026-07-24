using UnityEngine;
using TMPro; // Untuk UI Text
using DG.Tweening; // Untuk efek Shake

public class HealthManager : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 30;
    private int currentHP;

    [Header("UI & Effects")]
    public TMP_Text hpText; // Masukkan teks "HP: X" ke sini
    public GameObject hitEffectPrefab; // Masukkan prefab animasi 2D impact-mu ke sini

    private void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    // Fungsi ini dipanggil saat target terkena kartu serangan
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0; // Pastikan HP tidak minus
        
        UpdateUI();

        // --- EFEK VISUAL ---
        
        // 1. Shake / Guncangan objek (durasi 0.3 detik, kekuatan 0.5)
        transform.DOComplete(); // Hentikan animasi shake sebelumnya jika di-spam
        transform.DOShakePosition(0.3f, strength: 0.5f, vibrato: 10);

        // 2. Munculkan efek 2D Impact di posisi target
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // Cek apakah target mati
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHP}";
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} telah dikalahkan!");
        // Hancurkan objek setelah jeda sedikit agar efek shake terlihat dulu
        Destroy(gameObject, 0.5f); 
    }
}