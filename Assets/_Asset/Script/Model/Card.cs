

using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public int Mana { get; private set; }
    public int Damage;
    private readonly CardData data;

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
        Damage = cardData.Damage;
    }
}
