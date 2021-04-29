using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FamilyHappiness : MonoBehaviour
{


    [Header("Family Info")]
    [SerializeField] private Image IMG_Head;
    [SerializeField] private Image IMG_Emoji;
    [SerializeField] private Button enjoyCompanyButton;
    [SerializeField] private TextMeshProUGUI TXT_EnjoyText;
    [SerializeField] private TextMeshProUGUI TXT_TimeCost;
    [SerializeField] private TextMeshProUGUI TXT_FamiliarName;
    [SerializeField] private RectTransform foodBar;
    [SerializeField] private RectTransform heatBar;
    [SerializeField] private RectTransform pillBar;
    [SerializeField] private float enjoyMultiplier;

    [Header("President Info")]
    [SerializeField] private RectTransform presidentFoodBar;
    [SerializeField] private RectTransform presidentHeatBar;
    [SerializeField] private RectTransform presidentPillBar;

    private Vector2 presidentTotalBarSize;
    private Vector2 totalBarSize;

    private Familiar actualFamiliar;
    private FamiliarAction actualAction;



    private void Start()
    {
        totalBarSize = foodBar.sizeDelta;
        presidentTotalBarSize = presidentFoodBar.sizeDelta;
        enjoyCompanyButton.onClick.AddListener(delegate { EnjoyCompany(); });
    }

    public void ShowFamiliar(Familiar familiar)
    {
        actualFamiliar = familiar;

        if (actualFamiliar.enjoyedCompany)
        {
            enjoyCompanyButton.interactable = false;
        }
        else
        {
            enjoyCompanyButton.interactable = true;
        }

        int happiness = familiar.happiness;

        if (!actualFamiliar.enjoyedCompany)
            happiness = Mathf.RoundToInt(happiness / enjoyMultiplier);

        actualFamiliar.happiness = happiness;

        IMG_Emoji.sprite = Tyrannus.GetCorrectEmoji(actualFamiliar.happiness);

        TXT_FamiliarName.text = familiar.fullName;
        IMG_Head.sprite = familiar.avatarHead;

        int randomAction = Random.Range(0, familiar.actions.actions.Length);

        actualAction = familiar.actions.actions[randomAction];

        TXT_EnjoyText.text = actualAction.action;
        TXT_TimeCost.text = actualAction.timeCost.ToString() + "h";

        UpdateBars(totalBarSize, foodBar, actualFamiliar.maxDaysHungry, actualFamiliar.daysHungry);
        UpdateBars(totalBarSize, heatBar, actualFamiliar.maxDaysCold, actualFamiliar.daysCold);
        UpdateBars(totalBarSize, pillBar, actualFamiliar.maxDaysIll, actualFamiliar.daysIll);

    }

    public void UpdatePresidentInfo()
    {
        Familiar president = GameManager._instance.president;

        UpdateBars(presidentTotalBarSize, presidentFoodBar, president.maxDaysHungry, president.daysHungry);
        UpdateBars(presidentTotalBarSize, presidentHeatBar, president.maxDaysCold, president.daysCold);
        UpdateBars(presidentTotalBarSize, presidentPillBar, president.maxDaysIll, president.daysIll);
    }

    private void UpdateBars(Vector2 totalBar, RectTransform bar, int max, int actual)
    {
        Vector2 size = totalBar;

        float multiplier = ((float)(max - actual) / max);

        size.y = multiplier * totalBarSize.y;

        bar.sizeDelta = size;
    }

    public void EnjoyCompany()
    {
        actualFamiliar.happiness *= Mathf.RoundToInt(enjoyMultiplier);
        IMG_Emoji.sprite = Tyrannus.GetCorrectEmoji(actualFamiliar.happiness);
        actualFamiliar.enjoyedCompany = true;
        enjoyCompanyButton.interactable = false;

        GameManager._instance.timeManager.RestTime(actualAction.timeCost);
    }

}
