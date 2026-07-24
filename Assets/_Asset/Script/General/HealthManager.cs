using UnityEngine;
using TMPro; 
using DG.Tweening; 

public class HealthManager : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 30;
    private int currentHP;
    private int currentShield; // Tambahkan variabel pelindung

    [Header("UI & Effects")]
    public TMP_Text hpText; 
    public GameObject hitEffectPrefab; 

    private void Start()
    {
        currentHP = maxHP;
        currentShield = 0;
        UpdateUI();
    }

    // Fungsi untuk menambah darah (Heal)
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP; // Darah tidak boleh melebihi Max HP
        UpdateUI();
    }

    // Fungsi untuk menambah pelindung (Shield)
    public void AddShield(int amount)
    {
        currentShield += amount;
        UpdateUI();
    }

    // (Opsional) Fungsi untuk mereset Shield. 
    // Di game deckbuilder, biasanya shield hangus di awal giliran baru.
    public void ResetShield()
    {
        currentShield = 0;
        UpdateUI();
    }

    // Logika Take Damage yang diperbarui
    public void TakeDamage(int damage)
    {
        // 1. Coba tahan pakai Shield dulu
        if (currentShield > 0)
        {
            if (currentShield >= damage)
            {
                currentShield -= damage; // Shield cukup menahan semua damage
                damage = 0; 
            }
            else
            {
                damage -= currentShield; // Shield pecah, sisa damage tembus ke HP
                currentShield = 0;
            }
        }

        // 2. Jika masih ada sisa damage, kurangi HP
        if (damage > 0)
        {
            currentHP -= damage;
            if (currentHP < 0) currentHP = 0; 
            
            // --- EFEK VISUAL HANYA JIKA TEMBUS KE HP ---
            transform.DOComplete(); 
            transform.DOShakePosition(0.3f, strength: 0.5f, vibrato: 10);

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
        }
        
        UpdateUI();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (hpText != null)
        {
            // Logika UI teks gabungan
            if (currentShield > 0)
            {
                hpText.text = $"HP: {currentHP} + {currentShield}"; // Munculkan + shield
            }
            else
            {
                hpText.text = $"HP: {currentHP}"; // Normal
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} telah dikalahkan!");
        Destroy(gameObject, 0.5f); 
    }
}