using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singelton<DeckManager>
{
    [Header("Deck Setup")]
    [SerializeField] private List<CardData> startingDeckData;

    [Header("References")]
    [SerializeField] private HandView handView;

    // Tiga daftar utama untuk menyimpan wujud "hidup" dari kartu (Class Card, bukan CardData)
    private List<Card> drawPile = new();
    private List<Card> hand = new();
    private List<Card> discardPile = new();

    private void Start()
    {
        InitializeDeck();
    }

    // Fungsi untuk menyusun dek di awal permainan
    public void InitializeDeck()
    {
        foreach (CardData data in startingDeckData)
        {
            // Mengubah CardData (statis) menjadi Card (dinamis) lalu dimasukkan ke Draw Pile
            drawPile.Add(new Card(data));
        }

        ShuffleDeck(drawPile);
        Debug.Log($"Deck siap! Total kartu: {drawPile.Count}");
    }
    // Fungsi utama untuk menarik kartu
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // 1. Cek apakah Draw Pile kosong?
            if (drawPile.Count == 0)
            {
                // Jika Discard Pile juga kosong, hentikan penarikan (pemain kehabisan kartu)
                if (discardPile.Count == 0)
                {
                    Debug.Log("Tidak ada kartu tersisa untuk ditarik!");
                    break;
                }

                // Jika Draw Pile kosong tapi Discard Pile ada isinya, lakukan Reshuffle!
                ReshuffleDiscardIntoDraw();
            }

            // 2. Ambil kartu dari urutan paling atas (index 0)
            Card drawnCard = drawPile[0];
            drawPile.RemoveAt(0); // Hapus dari Draw Pile
            hand.Add(drawnCard);  // Pindahkan data ke Hand

            // 3. Panggil sistem UI buatanmu untuk memunculkan visualnya di layar
            // Posisi awal (transform.position) bisa kamu atur di posisi DeckManager berada.
            CardViews newCardView = CardViewCreator.Instance.CreateCardView(drawnCard, transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(newCardView));
        }
    }
    // Mekanik inti ala Slay the Spire: Daur ulang kartu
    private void ReshuffleDiscardIntoDraw()
    {
        Debug.Log("Mengacak ulang Discard Pile ke Draw Pile...");
        drawPile.AddRange(discardPile); // Pindahkan semua isi discard ke draw
        discardPile.Clear();            // Kosongkan discard pile
        ShuffleDeck(drawPile);          // Acak tumpukan yang baru
    }

    // Algoritma standar industri untuk mengacak List (Fisher-Yates Shuffle)
    private void ShuffleDeck(List<Card> deckToShuffle)
    {
        for (int i = 0; i < deckToShuffle.Count; i++)
        {
            int randomIndex = Random.Range(i, deckToShuffle.Count);
            Card temp = deckToShuffle[i];
            deckToShuffle[i] = deckToShuffle[randomIndex];
            deckToShuffle[randomIndex] = temp;
        }
    }
}
