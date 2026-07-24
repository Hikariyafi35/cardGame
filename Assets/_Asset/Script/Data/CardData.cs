using UnityEngine;

public enum CardType { Attack, Buff }
[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public CardType Type { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int HealAmount { get; private set; } 
    [field: SerializeField] public int ShieldAmount { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

}
