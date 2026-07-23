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
        Debug.Log("Giliran Musuh...");

        // Simulasi waktu musuh berpikir dan menyerang (jeda 2 detik)
        yield return new WaitForSeconds(2f);
        
        Debug.Log("Musuh selesai menyerang!");

        // Setelah musuh selesai, kembalikan ke giliran pemain
        StartPlayerTurn();
    }
}