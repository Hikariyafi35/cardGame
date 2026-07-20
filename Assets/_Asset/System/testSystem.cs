using Unity.Mathematics;
using UnityEngine;

public class testSystem : MonoBehaviour
{
    [SerializeField] private HandView handView;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CardViews cardViews = CardViewCreator.Instance.CreateCardView(transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardViews));
        }
    }
}
