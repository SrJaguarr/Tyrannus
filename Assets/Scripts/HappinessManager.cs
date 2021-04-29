using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TXT_CityHappiness, TXT_FamilyHappiness, TXT_GlobalHappiness, TXT_PresidentHappiness;
    private SociaCategory[] categories;

    private Familiar[] family;

    [SerializeField] private Image EMOJI_City, EMOJI_Family, EMOJI_Global;

    public int cityHappiness;
    public int familyHappiness;
    public int globalHappiness;
    private int totalPercentage;

    private float handicapPercentage;
    List<float> loanPenalties;

    private void Awake()
    {
        loanPenalties = new List<float>();

        categories = GameManager._instance.socialCategoryDB.categories;
        family = GameManager._instance.familyDB.familiars;

        CalculateCitizenPercentage();
    }

    public void NewGame()
    {
        ClearPenalties();
        CalculateGlobalHappiness();
    }

    private void CalculateCitizenPercentage()
    {
        for(int i = 0; i < categories.Length; i++)
        {
            totalPercentage += categories[i].populationPercentage;
        }
    }

    #region Categories Opinion About Request

    public List<SociaCategory> GetAffectedCategories(Request request)
    {
        List<SociaCategory> affectedCategories = new List<SociaCategory>();

        for(int i = 0; i < categories.Length; i++)
        {
            if(Array.Exists(categories[i].requestsToApprove, element => element == request) || Array.Exists(categories[i].RequestsToAbolish, element => element == request))
            {
                affectedCategories.Add(categories[i]);
            }
        }

        return affectedCategories;
    }

    #endregion

    public int CalculateSocialCategoryHappines(SociaCategory socialCategory)
    {
        int totalRequests = socialCategory.requestsToApprove.Length + socialCategory.RequestsToAbolish.Length;
        int sum = 0;

        for(int i = 0; i < socialCategory.requestsToApprove.Length; i++)    //Recorremos todas las medidas que quieren ver aprobadas
        {
            int requestHappiness = socialCategory.requestsToApprove[i].level * 25;  //*25 -> 100% / 4
            sum += requestHappiness;
        }

        for(int i = 0; i < socialCategory.RequestsToAbolish.Length; i++)
        {
            int requestHappiness = Math.Abs(socialCategory.RequestsToAbolish[i].level * 25 - 100);
            sum += requestHappiness;
        }


        return socialCategory.happiness = sum / totalRequests;
    }

    public void CalculateCityHappiness()
    {
        cityHappiness = 0;

        for(int i = 0; i < categories.Length; i++)
        {
            CalculateSocialCategoryHappines(categories[i]);
            cityHappiness += categories[i].happiness * categories[i].populationPercentage;
        }

        cityHappiness = cityHappiness / totalPercentage;

        for(int i = 0; i < loanPenalties.Count; i++)
        {
            cityHappiness -= (int)(cityHappiness * loanPenalties[i]);
        }

        cityHappiness -= (int)(cityHappiness * handicapPercentage);
    }

    public void UpdateCitizenHappiness(Citizen citizen)
    {
        int totalSocialCategories = citizen.socialCategories.Length;
        int sum = 0;

        for(int i = 0; i < totalSocialCategories; i++)
        {
            sum += citizen.socialCategories[i].happiness;
        }

        citizen.citizenHappiness = sum / totalSocialCategories;
    }

    public void UpdateHappinessLabel()
    {
        EMOJI_Global.sprite = Tyrannus.GetCorrectEmoji(globalHappiness);
        EMOJI_City.sprite = Tyrannus.GetCorrectEmoji(cityHappiness);
        EMOJI_Family.sprite =Tyrannus.GetCorrectEmoji(familyHappiness);

        TXT_PresidentHappiness.text = TXT_GlobalHappiness.text = globalHappiness.ToString() + "%";
        TXT_CityHappiness.text = cityHappiness.ToString() + "%";
        TXT_FamilyHappiness.text = familyHappiness.ToString() + "%";

    }

    public void AddLoanPenalty(float f)
    {
        loanPenalties.Add(f);
    }

    public void ClearPenalties()
    {
        loanPenalties.Clear();
    }

    public void SetHandicap(float f)
    {
        handicapPercentage = f;
    }

    public int GetFamiliarHappiness(Familiar actualFamiliar)
    {

        if (actualFamiliar.daysCold == actualFamiliar.maxDaysCold || actualFamiliar.daysHungry == actualFamiliar.maxDaysHungry || actualFamiliar.daysIll == actualFamiliar.maxDaysIll)
        {
            return 0;
        }

        float foodHappiness = 33;
        float heatHappiness = 33;
        float illnessHappiness = 34;

            foodHappiness *= ((float)(actualFamiliar.maxDaysHungry - actualFamiliar.daysHungry) / actualFamiliar.maxDaysHungry);
            heatHappiness *= ((float)(actualFamiliar.maxDaysCold - actualFamiliar.daysCold) / actualFamiliar.maxDaysCold);
            illnessHappiness *= ((float)(actualFamiliar.maxDaysIll - actualFamiliar.daysIll) / actualFamiliar.maxDaysIll);


        int happiness = Mathf.RoundToInt(foodHappiness + heatHappiness + illnessHappiness);

        return happiness;
    }

    public int CalculateFamilyHappiness()
    {
        familyHappiness = 0;

        for(int i = 1; i < family.Length; i++)
        {
            family[i].happiness = GetFamiliarHappiness(family[i]);
            familyHappiness += family[i].happiness;
        }

        familyHappiness = familyHappiness / (family.Length - 1);

        return familyHappiness;
    }

    public void CalculateGlobalHappiness()
    {
        //felicidad del presidente que mostrara como vamos en el juego (teniendo en cuenta que la felicidad de la ciudad no podrá subir mucho mas de 52%)
        // 34% mis necesidades
        // 33% mi familia
        // 33% mis ciudadanos

        Familiar president = GameManager._instance.president;

        CalculateCityHappiness();
        familyHappiness = CalculateFamilyHappiness();

        int needsAmount = Mathf.RoundToInt(GetFamiliarHappiness(president) * 0.34f);

        if(president.daysCold == president.maxDaysCold || president.daysHungry == president.maxDaysHungry || president.daysIll == president.maxDaysIll || president.happiness <= GameManager._instance.minimumHappiness)
        {
            GameManager._instance.GameOver();
            return;
        }

        int familyAmount = Mathf.RoundToInt(familyHappiness * 0.33f);
        int cityAmount = Mathf.RoundToInt(cityHappiness * 0.33f);

        globalHappiness = needsAmount + familyAmount + cityAmount;

        UpdateHappinessLabel();

    }
}
