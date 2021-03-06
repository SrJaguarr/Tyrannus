using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestStats : MonoBehaviour
{
    private SocialCategoryDB socialCategoryDB;

    [SerializeField] private GameObject affectedRequestPrefab, affectedSCPrefab;
    [SerializeField] private Transform requestsLayout, categoriesLayout;

    [SerializeField] private TextMeshProUGUI TXT_RemainingChanges, TXT_RequestTitle, TXT_CategoryTitle, TXT_CategoryDescription, TXT_Balance, TXT_NewBalance;
    [SerializeField] private Image SCImage, IMG_Icon;
    [SerializeField] private Slider requestSlider;

    private Request actualRequest;

    private int provisionalChanges;

    public int totalChanges;

    private int newRequestValue;
    private int remainingChanges;
    private int newBalance;

    private List<AffectedSCButton> affectedCategories;

    private void Awake()
    {
        affectedCategories = new List<AffectedSCButton>();
    }
    private void Start()
    {
        socialCategoryDB = GameManager._instance.socialCategoryDB;
        requestSlider.onValueChanged.AddListener(delegate { UpdateSlider(); });
    }

    public void NewDay()
    {
        remainingChanges = totalChanges;
        CleanRequests();
        CleanCategories();
    }

    public void ShowRequestList(SociaCategory sc)
    {
        SCImage.sprite = sc.categorySprite;
        TXT_CategoryTitle.text = sc.categoryName;
        TXT_CategoryDescription.text = sc.categoryDescription;

        for(int i = 0; i < sc.RequestsToAbolish.Length; i++)
        {
            GameObject button = Instantiate(affectedRequestPrefab, requestsLayout);
            button.GetComponent<StatRequestButton>().InitializeSelf(sc.RequestsToAbolish[i], false);
        }

        for (int i = 0; i < sc.requestsToApprove.Length; i++)
        {
            GameObject button = Instantiate(affectedRequestPrefab, requestsLayout);
            button.GetComponent<StatRequestButton>().InitializeSelf(sc.requestsToApprove[i], true);
        }
    }

    public void ShowAffectedCategories(Request r)
    {
        newBalance = GameManager._instance.moneyManager.incoming;
        TXT_Balance.text = newBalance.ToString() + "€";
        TXT_NewBalance.text = newBalance.ToString() + "€";

        TXT_RequestTitle.text = r.requestName;
        IMG_Icon.sprite = r.icon;
        TXT_RemainingChanges.text = remainingChanges.ToString();
        actualRequest = r;
        
        requestSlider.value = r.level;
        newRequestValue = (int)requestSlider.value;

        //Cargamos todas las categorias sociales afectadas
        for (int i = 0; i < socialCategoryDB.categories.Length; i++)
        {
            SociaCategory sc = socialCategoryDB.categories[i];

            if (Tyrannus.IsApproval(sc, r) || Tyrannus.IsAbolishion(sc, r))
            {
                GameObject button = Instantiate(affectedSCPrefab, categoriesLayout);
                button.GetComponent<AffectedSCButton>().InitializeSelf(sc, r);

                affectedCategories.Add(button.GetComponent<AffectedSCButton>());
            }
        }

    }

    public void CleanRequests()
    {
        Tyrannus.CleanParent(requestsLayout);
    }

    public void CleanCategories()
    {
        affectedCategories.Clear();
        Tyrannus.CleanParent(categoriesLayout);
    }

    private void UpdateSlider()
    {
        newBalance = GameManager._instance.moneyManager.incoming;

        provisionalChanges = Mathf.Abs(actualRequest.level - (int)requestSlider.value);    // ------- 1 ------------- 5  == 4

        if(provisionalChanges <= remainingChanges)                                             // 4 <= 5
        {
            provisionalChanges = remainingChanges - provisionalChanges;                                                //5 - 4 = 1
            newRequestValue = (int)requestSlider.value;

            if(actualRequest.costPerLevel.Length > 0)
                newBalance = newBalance + actualRequest.costPerLevel[actualRequest.level] - actualRequest.costPerLevel[newRequestValue];

            TXT_NewBalance.text = newBalance.ToString() + "€";
        }
        else
        {
            requestSlider.value = newRequestValue;
        }


        TXT_RemainingChanges.text = provisionalChanges.ToString();

        foreach(AffectedSCButton category in affectedCategories)
        {
            category.UpdateLevel(newRequestValue);
        }

        UpdateIncomingColor();

    }

    public void ClearChanges()
    {
        requestSlider.value = actualRequest.level;
        newRequestValue = actualRequest.level;
        
        TXT_RemainingChanges.text = remainingChanges.ToString();

        UpdateIncomingColor();
    }

    public void ResetChanges()
    {
        remainingChanges = totalChanges;
        TXT_RemainingChanges.text = remainingChanges.ToString();

        UpdateIncomingColor();
    }

    public void ConfirmChanges()
    {
        actualRequest.level = newRequestValue;
        remainingChanges = provisionalChanges;

        UpdateIncomingColor();
    }

    private void UpdateIncomingColor()
    {
        TXT_NewBalance.color = Color.black;

        if(newBalance < GameManager._instance.moneyManager.incoming)
        {
            TXT_NewBalance.color = "B52F30".ToColor();
        }

        if (newBalance > GameManager._instance.moneyManager.incoming)
        {
            TXT_NewBalance.color = "2C7556".ToColor();
        }
    }

    public bool HasChanges() 
    {
        return newRequestValue != actualRequest.level;
    }
}
