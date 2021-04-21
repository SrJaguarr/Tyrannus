using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HappinessManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TXT_GlobalHappiness;
    private SociaCategory[] categories;
    private Citizen[] citizens;

    public int globalHappiness;
    private int totalPercentage;

    private float handicapPercentage;
    List<float> loanPenalties;

    private void Awake()
    {
        loanPenalties = new List<float>();

        categories = GameManager._instance.socialCategoryDB.categories;
        citizens = GameManager._instance.citizenDB.citizens;

        CalculateGlobalPercentage();
    }

    public void NewGame()
    {
        CalculateGlobalHappiness();
        ClearPenalties();
    }

    private void CalculateGlobalPercentage()
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

    public void CalculateGlobalHappiness()
    {
        globalHappiness = 0;

        for(int i = 0; i < categories.Length; i++)
        {
            CalculateSocialCategoryHappines(categories[i]);
            globalHappiness += categories[i].happiness * categories[i].populationPercentage;
        }

        globalHappiness = globalHappiness / totalPercentage;

        for(int i = 0; i < loanPenalties.Count; i++)
        {
            globalHappiness -= (int)(globalHappiness * loanPenalties[i]);
        }

        globalHappiness -= (int)(globalHappiness * handicapPercentage);

        UpdateHappinessLabel();
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
        TXT_GlobalHappiness.text = globalHappiness.ToString() + "%";

        GameManager._instance.canvasManager.SetHappinessEmoji(Tyrannus.GetCorrectEmoji(globalHappiness));

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
}
