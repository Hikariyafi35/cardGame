using System.Collections;
using UnityEngine;

public class TurnManager : Singelton<TurnManager>
{
    // Status giliran saat ini
    public enum TurnState { PlayerTurn, EnemyTurn }
    public TurnState currentState;

    private IEnumerator Start()
    {
        // Tunggu sebentar agar DeckManager selesai menyiapkan kartu
        yield return new WaitForSeconds(0.1f); 
        
        // Setelah aman, baru mulai giliran pemain
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        currentState = TurnState.PlayerTurn;
        Debug.Log("--- GILIRAN PEMAIN DIMULAI ---");

        // --- HAPUS SHIELD LAMA ---
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<HealthManager>().ResetShield();

        // 1. Reset Mana (Panggil dari ManaManager milikmu)
        if (ManaManager.Instance != null)
        {
            ManaManager.Instance.ResetMana();
        }

        // 2. Tarik 5 kartu baru dari dek
        if (DeckManager.Instance != null)
        {
            DeckManager.Instance.DrawCards(5);
        }
    }

    // Fungsi ini akan dipanggil oleh Tombol "End Turn" di UI
    public void EndPlayerTurn()
    {
        // Jangan lakukan apa-apa jika ini bukan giliran pemain
        if (currentState != TurnState.PlayerTurn) return;

        Debug.Log("--- GILIRAN PEMAIN BERAKHIR ---");
        
        // 1. Sapu bersih kartu yang tersisa di tangan
        HandView handView = FindAnyObjectByType<HandView>();
        if (handView != null)
        {
            handView.ClearHand();
        }

        // 2. Mulai giliran musuh
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        currentState = TurnState.EnemyTurn;
        Debug.Log("--- GILIRAN MUSUH DIMULAI ---");

        // Cari semua musuh yang ada di scene (yang memiliki script EnemyBrain)
        EnemyBrain[] enemies = FindObjectsByType<EnemyBrain>(FindObjectsSortMode.None);

        if (enemies.Length > 0)
        {
            // Suruh musuh menyerang satu per satu secara berurutan
            foreach (EnemyBrain enemy in enemies)
            {
                if (enemy != null) // Pastikan musuhnya belum mati
                {
                    // Tunggu sampai musuh ini selesai menyerang, baru lanjut ke musuh berikutnya
                    yield return StartCoroutine(enemy.TakeTurn()); 
                }
            }
        }
        else
        {
            // Jika tidak ada musuh, tunggu sebentar saja
            yield return new WaitForSeconds(1f);
            Debug.Log("Tidak ada musuh di arena.");
        }
        
        Debug.Log("--- GILIRAN MUSUH BERAKHIR ---");

        // Setelah semua musuh selesai, kembalikan ke giliran pemain
        StartPlayerTurn();
    }
}