using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CardViewCreator : Singelton<CardViewCreator>
{
    [SerializeField] private CardViews cardViewsPrefabs;

    public CardViews CreateCardView(Vector3 position,Quaternion rotation)
    {
        CardViews cardViews = Instantiate(cardViewsPrefabs, position, rotation);
        cardViews.transform.localScale = Vector3.zero;
        cardViews.transform.DOScale(Vector3.one, 0.15f);
        return cardViews;
    }
}
