using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ManaManager : Singelton<ManaManager>
{
    [Header("Mana Settings")]
    public int maxMana = 3;
    private int currentMana;

    [Header("UI Reference")]
    [SerializeField] private TMP_Text manaText;

    private void Start()
    {
        ResetMana();
    }
    public void ResetMana()
    {
        currentMana = maxMana;
        UpdateUI();
    }
    public bool TryUseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        Debug.Log("Mana tidak cukup");
        return false;
    }
    private void UpdateUI()
    {
        if(manaText != null)
        {
            manaText.text = $"{currentMana}/{maxMana}";
        }
    }
}
