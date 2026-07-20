using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CardViewCreator : Singelton<CardViewCreator>
{
    [SerializeField] private CardViews cardViewsPrefabs;

    public CardViews CreateCardView(Card card,Vector3 position,Quaternion rotation)
    {
        CardViews cardViews = Instantiate(cardViewsPrefabs, position, rotation);
        cardViews.transform.localScale = Vector3.zero;// Set ukuran awal jadi 0 (tidak terlihat)
        // Gunakan DOTween untuk mengubah ukuran ke 1 (Vector3.one) dalam waktu 0.15 detik
        cardViews.transform.DOScale(Vector3.one, 0.15f);
        cardViews.Setup(card);
        return cardViews;
    }
}
