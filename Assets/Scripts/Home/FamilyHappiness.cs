using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FamilyHappiness : MonoBehaviour
{
    [SerializeField] private Image IMG_Head, IMG_Emoji;

    [SerializeField] private Button enjoyCompanyButton;

    [SerializeField] private TextMeshProUGUI TXT_EnjoyText, TXT_TimeCost, TXT_FamiliarName;

    [SerializeField] private RectTransform foodBar, heatBar, pillBar;
    private Vector2 totalBarSize;

    private Familiar actualFamiliar;
    private FamiliarAction actualAction;

    [SerializeField] private float enjoyMultiplier;

    private void Start()
    {
        totalBarSize = foodBar.sizeDelta;
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


        CalculateFamiliarHappiness();

        IMG_Emoji.sprite = Tyrannus.GetCorrectEmoji(actualFamiliar.happiness);

        TXT_FamiliarName.text = familiar.fullName;
        IMG_Head.sprite = familiar.avatarHead;

        int randomAction = Random.Range(0, familiar.actions.actions.Length);

        actualAction = familiar.actions.actions[randomAction];

        TXT_EnjoyText.text = actualAction.action;
        TXT_TimeCost.text = actualAction.timeCost.ToString() + "h";

        UpdateBars(foodBar, actualFamiliar.maxDaysHungry, actualFamiliar.daysHungry);
        UpdateBars(heatBar, actualFamiliar.maxDaysCold, actualFamiliar.daysCold);
        UpdateBars(pillBar, actualFamiliar.maxDaysIll, actualFamiliar.daysIll);

    }

    private void UpdateBars(RectTransform bar, int max, int actual)
    {
        Vector2 size = totalBarSize;

        float multiplier = ((float)(max - actual) / max);

        size.y = multiplier * totalBarSize.y;

        bar.sizeDelta = size;
    }

    private void CalculateFamiliarHappiness()
    {
        float foodHappiness = 33;
        float heatHappiness = 33;
        float illnessHappiness = 34;

        if(actualFamiliar.daysHungry > 0)
            foodHappiness *= ((float)actualFamiliar.daysHungry/ actualFamiliar.maxDaysHungry);

        if(actualFamiliar.daysCold > 0)
            heatHappiness *= ((float)actualFamiliar.daysCold / actualFamiliar.maxDaysCold);

        if(actualFamiliar.daysIll > 0)
            illnessHappiness *= ((float)actualFamiliar.daysIll / actualFamiliar.maxDaysIll);

        int happiness = Mathf.RoundToInt(foodHappiness + heatHappiness + illnessHappiness);

        if (!actualFamiliar.enjoyedCompany)
            happiness = Mathf.RoundToInt(happiness / enjoyMultiplier);

        actualFamiliar.happiness = happiness;
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
