using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopListController : MonoBehaviour
{
    public int foodPrice, pillPrice, heatingPrice, rentPrice;

    public Toggle toggleHeating, toggleRent;

    [SerializeField] Sprite spriteHeatOn, spriteHeatOff;
    [SerializeField] Image IMG_HeatImage;

    [SerializeField]
    private TextMeshProUGUI TXT_RentPrice, TXT_HeatingPrice, TXT_ShopResult, TXT_Saves;


    [SerializeField]
    private Transform shopListContainer;

    [SerializeField]
    private GameObject shopListFamiliarPrefab;

    private Dictionary<Toggle, int> toggles;
    private List<Toggle> boughtNeeds;

    private Dictionary<Familiar, GameObject> familiarContainers;

    private int shopPrice;
    private int money;
    private int balance;

    private bool paidRent;

    private MoneyManager moneyManager;

    private void Awake()
    {
        moneyManager = GameManager._instance.moneyManager;
        familiarContainers = new Dictionary<Familiar, GameObject>();
        toggles = new Dictionary<Toggle, int>();
        boughtNeeds = new List<Toggle>();
    }

    public void NewGame()
    {
        Tyrannus.CleanParent(shopListContainer);
        familiarContainers.Clear();
        shopPrice = rentPrice;
        paidRent = false;
    }

    private void Start()
    {
        AddToggle(toggleHeating, heatingPrice);

        TXT_HeatingPrice.text = heatingPrice.ToString() + "€";
        TXT_RentPrice.text = rentPrice.ToString() + "€"; 
        TXT_RentPrice.color = Color.red;
        TXT_RentPrice.text = "-" + rentPrice.ToString() + "€";

    }
    public void AddToggle(Toggle toggle, int i)
    {
        toggles.Add(toggle, i);
    }

    public void AddFamiliarToShopList(Familiar familiar)
    {
        shopListFamiliarPrefab.transform.GetChild(0).GetComponent<Image>().sprite = familiar.avatarHead;
        familiarContainers.Add(familiar, Instantiate(shopListFamiliarPrefab, shopListContainer));
    }

    #region Salary
    public void UpdateSalary()
    {
        money = moneyManager.salary;
        balance = money - shopPrice;

        TXT_Saves.text = money.ToString() + "€";
        TXT_ShopResult.text = balance.ToString() + "€";
    }

    public void CheckToggles()
    {
        foreach (KeyValuePair<Toggle, int> need in toggles)
        {
            if (!boughtNeeds.Contains(need.Key))
            {
                if (need.Key.isOn)
                {
                    if (balance - need.Value < 0)
                    {
                        need.Key.interactable = false;
                    }
                    else
                    {
                        need.Key.interactable = true;
                    }
                }
            }
            else
            {
                need.Key.interactable = false;
            }



        }
    }

    private void BuyNeeds()
    {
        foreach (KeyValuePair<Toggle, int> need in toggles)
        {
            if (!need.Key.isOn)
            {
                boughtNeeds.Add(need.Key);
            }
        }
        CheckToggles();
    }

    public void PayHeating()
    {
        if (toggleHeating.isOn)
        {
            TXT_HeatingPrice.color = Color.black;
            TXT_HeatingPrice.text = heatingPrice.ToString() + "€";
            IMG_HeatImage.sprite = spriteHeatOff;

            SetShopPrice(-heatingPrice);
        }
        else
        {
            TXT_HeatingPrice.color = Color.red;
            TXT_HeatingPrice.text = "-" + heatingPrice.ToString() + "€";
            IMG_HeatImage.sprite = spriteHeatOn;

            SetShopPrice(heatingPrice);
        }

        CheckToggles();
    }

    public void SetShopPrice(int cost)
    {
        shopPrice += cost;
        UpdateSalary();
    }

    public void Buy()
    {
        moneyManager.Shop(shopPrice);
        BuyNeeds();
        shopPrice = 0;
        UpdateSalary();
        paidRent = true;
    }
    private void ResetToggles()
    {
        foreach (KeyValuePair<Toggle, int> toggle in toggles)
        {
            toggle.Key.isOn = true;
        }
    }

    public void ClearBoughtNeeds()
    {
        ResetToggles();
        boughtNeeds.Clear();
        CheckToggles();

        shopPrice = rentPrice;
        CheckRent();
        UpdateSalary();
        paidRent = false;
    }

    private void CheckRent()
    {
        if (!paidRent)
        {
            moneyManager.salary -= rentPrice;
        }
    }

    #endregion
    #region Needs
        #region Check isOn

        public bool IsFoodOn(Familiar familiar)
        {
            familiarContainers.TryGetValue(familiar, out GameObject familiarContainer);
            return !familiarContainer.transform.GetChild(1).GetComponent<Toggle>().isOn;
        }

        public bool IsPillOn(Familiar familiar)
        {
            familiarContainers.TryGetValue(familiar, out GameObject familiarContainer);
            return !familiarContainer.transform.GetChild(2).GetComponent<Toggle>().isOn;
        }
    #endregion
        public void ShowPills(Familiar familiar, bool show)
        {
            familiarContainers.TryGetValue(familiar, out GameObject familiarContainer);
            familiarContainer.transform.GetChild(2).gameObject.SetActive(show);
        }

        public void FamiliarDeletion(Familiar familiar)
        {
            familiarContainers.TryGetValue(familiar, out GameObject familiarContainer);
            familiarContainer.transform.GetChild(3).gameObject.SetActive(true);
        }
    #endregion



}
