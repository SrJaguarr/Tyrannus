using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TXT_FoodPrice, TXT_PillPrice;

    private int foodPrice, pillPrice;
    private ShopListController shopListController;

    [SerializeField]
    private Toggle toggleFood, togglePill;
    private void Awake()
    {
        shopListController = GameManager._instance.shopListController;
    }

    private void Start()
    {
        foodPrice = shopListController.foodPrice;
        pillPrice = shopListController.pillPrice;

        shopListController.AddToggle(toggleFood, foodPrice);
        shopListController.AddToggle(togglePill, pillPrice);

        TXT_FoodPrice.text = foodPrice.ToString() + "€";
        TXT_PillPrice.text = pillPrice.ToString() + "€";
    }

    public void PayFood()
    {
        if (toggleFood.isOn)
        {
            TXT_FoodPrice.color = Color.black;
            TXT_FoodPrice.text = foodPrice.ToString() + "€";
            shopListController.SetShopPrice(-foodPrice);
        }
        else
        {
            TXT_FoodPrice.color = "B52F30".ToColor();
            TXT_FoodPrice.text = "-" + foodPrice.ToString() + "€";
            shopListController.SetShopPrice(foodPrice);
            GameManager._instance.fxManager.PlaySound("add_item");
        }

        shopListController.CheckToggles();
    }

    public void PayPills()
    {
        if (togglePill.isOn)
        {
            TXT_PillPrice.color = Color.black;
            TXT_PillPrice.text = pillPrice.ToString() + "€";
            shopListController.SetShopPrice(-pillPrice);
            GameManager._instance.fxManager.PlaySound("add_item");
        }
        else
        {
            TXT_PillPrice.color = "B52F30".ToColor();
            TXT_PillPrice.text = "-" + pillPrice.ToString() + "€";
            shopListController.SetShopPrice(pillPrice);
        }
        
        shopListController.CheckToggles();
    }
}

