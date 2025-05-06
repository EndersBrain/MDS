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
            playerMoney -= item.itemCost;
            toolBarManager.AddItem(item);
            Debug.Log($"Bought {item.name} for {item.itemCost} money. Remaining money: {playerMoney}");
            UpdateMoneyText();
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
