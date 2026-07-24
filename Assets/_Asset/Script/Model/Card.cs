

using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public int Mana { get; private set; }
    public int Damage { get; private set; }

    public CardType Type { get; private set; }
    public int HealAmount { get; private set; }
    public int ShieldAmount { get; private set; }

    private readonly CardData data;

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
        Damage = cardData.Damage;
        
        Type = cardData.Type;
        HealAmount = cardData.HealAmount;
        ShieldAmount = cardData.ShieldAmount;
    }
}
