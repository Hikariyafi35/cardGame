using Unity.Mathematics;
using UnityEngine;

public class testSystem : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private CardData cardData;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Card card = new(cardData);
            CardViews cardViews = CardViewCreator.Instance.CreateCardView(card, transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardViews));
        }
    }
}
