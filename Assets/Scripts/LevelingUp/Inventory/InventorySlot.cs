using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public UpgradeItem item { get; private set; }
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Image icon;

    public void SetItem(UpgradeItem item)
    {
        this.item = item;
        icon.sprite = item.icon;
    }

    public void UpdateSlot(Hero hero)
    {
        if (item == null) return;

        amountText.text = $"{hero.Stats.Mediator.GetModifierStacks(item.name)} / {item.maxStacks}";
    }
}
