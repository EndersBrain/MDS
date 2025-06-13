using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShopPopUpManager : MonoBehaviour
{
    public GameObject shoppopupPanel;
    public int playerMoney = 1000;
    public Text moneyText;

    void Start()
    {
        shoppopupPanel.SetActive(false);
        UpdateMoneyText();
    }

    public void ShowPopup()
    {
        shoppopupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        shoppopupPanel.SetActive(false);
    }

    public void TogglePopup()
    {
        shoppopupPanel.SetActive(!shoppopupPanel.activeSelf);
    }

    public void BuyItem(Item item)
    {
        ToolBarManager toolBarManager = FindFirstObjectByType<ToolBarManager>();
        if (playerMoney >= item.itemCost)
        {
            if (toolBarManager.AddItem(item) == false)
            {
                foreach (var slot in toolBarManager.toolbarSlots)
                {
                    ToolBarItem itemInSlot = slot.GetComponentInChildren<ToolBarItem>();
                    if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count > 0)
                    {
                        itemInSlot.count--;
                        itemInSlot.RefreshCount();
                        Debug.Log("Decreased item count in slot due to failed add.");
                        break;
                    }
                }
                Debug.Log("Failed to add item to toolbar.");
            }
            else
            {
                playerMoney -= item.itemCost;
                UpdateMoneyText();
                Debug.Log("Item bought: " + item.name);
            }
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = $"Money: {playerMoney}";
    }
}
