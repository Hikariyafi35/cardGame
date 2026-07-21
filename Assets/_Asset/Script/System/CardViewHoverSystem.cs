using UnityEngine;

public class CardViewHoverSystem : Singelton<CardViewHoverSystem>
{
    [SerializeField] private CardViews cardViewHover;

    public void Show(Card card, Vector3 position)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        cardViewHover.transform.position = position;
    }
    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }
}
