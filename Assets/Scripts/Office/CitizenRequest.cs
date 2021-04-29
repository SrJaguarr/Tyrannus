using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CitizenRequest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TXT_CitizenName, TXT_Request, TXT_Description, TXT_Age, TXT_Categories, TXT_Tip, TXT_Bubble;

    [SerializeField]
    private Transform levelsBefore, levelsAfter;

    [SerializeField]
    private Slider sliderBefore, sliderAfter;

    [SerializeField]
    private Image emojiBefore, emojiAfter, requestIcon, bubbleIcon;

    [SerializeField]
    private GameObject scPrefab;

    public int numberOfRequests = 3;
    private int actualCitizen = 0;

    private CitizenDB citizenList;

    private List<Citizen> dailyCitizens;
    private List<Request> dailyRequests;
    private List<SociaCategory> dailySocialCategories;

    [SerializeField] [Range(1, 10)] private int maxTip;
    private int tip;

    [SerializeField]
    private Button BTN_Citizen;

    [SerializeField]
    private Image headContainer;

    [SerializeField]
    Transform PNL_AcceptConditions, PNL_DenyConditions;

    private HappinessManager happinessManager;

    private void Start()
    {
        citizenList = GameManager._instance.citizenDB;

        happinessManager = GameObject.Find("Managers").gameObject.GetComponent<HappinessManager>();

        dailyCitizens = new List<Citizen>();
        dailyRequests = new List<Request>();
        dailySocialCategories = new List<SociaCategory>();

        CitizenSelector();
    }


    public void CitizenSelector()
    {
        actualCitizen = 0;

        BTN_Citizen.gameObject.SetActive(true);

        dailyCitizens.Clear();
        dailyRequests.Clear();
        dailySocialCategories.Clear();

        PickCitizens();
        PickRequests();
        ShowCitizen();
    }

    #region Pick Section

    #region Pick Valid Citizens
    private void PickCitizens()         //Selecciona el numero establecido de ciudadanos aleatorios no repetidos
    {
        Citizen[] citizens = citizenList.citizens;
        citizens = Tyrannus.ShuffleCitizens(citizens);

        for (int i = 0; i < numberOfRequests; i++)
        {
            foreach(Citizen citizen in citizens)
            {
                if (!dailyCitizens.Contains(citizen) && citizen.citizenHappiness < 100)
                {
                    dailyCitizens.Add(citizen);
                    happinessManager.UpdateCitizenHappiness(citizen);
                }
            }
        }

    }

    #endregion
    #region Pick Valid Requests
    private void PickRequests()
    {
        for(int i = 0; i < numberOfRequests; i++)
        {
            dailyRequests.Add(ExtractValidRequest(dailyCitizens[i]));
        }

    }

    private Request ExtractValidRequest(Citizen citizen)
    {
        Request validRequest = null;
        bool isApproval = (UnityEngine.Random.Range(0, 2) == 0);
        SociaCategory[] categories = citizen.socialCategories;

        categories = Tyrannus.ShuffleCategories(categories);

        while(validRequest == null)
        {
            if (isApproval)                                                                             //Si es propuesta de aprobacion
            {
                for (int i = 0; i < categories.Length; i++)                                               //Recorro cada categoria
                {
                    Request[] requests = categories[i].requestsToApprove;
                    requests = Tyrannus.ShuffleRequests(requests);

                    foreach (Request request in requests)                                                     //Compruebo si hay alguna categoria valida
                    {
                        if (!dailyRequests.Contains(request) && request.level < 4)
                        {
                            dailySocialCategories.Add(categories[i]);
                            validRequest = request;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < categories.Length; i++)                                               //Recorro cada categoria
                {
                    Request[] requests = categories[i].RequestsToAbolish;
                    requests = Tyrannus.ShuffleRequests(requests);

                    foreach (Request request in requests)                                                     //Compruebo si hay alguna categoria valida
                    {
                        if (!dailyRequests.Contains(request) && request.level > 0)
                        {
                            dailySocialCategories.Add(categories[i]);
                            validRequest = request;
                            break;
                        }
                    }
                }
            }

            isApproval = !isApproval;
        }

        return validRequest;
    }

    #endregion

    #endregion

    #region Show Actual Citizen
    private void ShowCitizen()                                  //Se muestra el ciudadano actual, asi como su peticion
    {
        string categories = "";
        Citizen citizen = dailyCitizens[actualCitizen];
        Request request = dailyRequests[actualCitizen];

        tip = UnityEngine.Random.Range(1, maxTip);

        headContainer.sprite = citizen.spriteHead;
        BTN_Citizen.image.sprite = citizen.sprite;
        TXT_CitizenName.text = citizen.fullName;
        TXT_Age.text = citizen.age.ToString() + " años";
        TXT_Tip.text = tip.ToString() + "€";

        for (int i = 0; i < citizen.socialCategories.Length; i++)
        {
            categories = categories + citizen.socialCategories[i].categoryName;
            if (i < citizen.socialCategories.Length - 1)
                categories = categories + ", ";
        }
        TXT_Categories.text = categories;

        TXT_Request.text = request.requestName;
        TXT_Description.text = request.description;

        int lvl = request.level;
        bool isApproval;

        if (Tyrannus.IsApproval(dailySocialCategories[actualCitizen], dailyRequests[actualCitizen]))
        {
            TXT_Description.text = request.approvalRequest[0];
            TXT_Bubble.text = "+";
            lvl++;
            isApproval = true;
        }
        else
        {
            TXT_Description.text = request.abolitionRequest[0];
            TXT_Bubble.text = "-";
            lvl--;
            isApproval = false;
        }

        emojiBefore.sprite = Tyrannus.GetCorrectEmoji(isApproval, request.level);
        emojiAfter.sprite = Tyrannus.GetCorrectEmoji(isApproval, lvl);

        sliderBefore.value = request.level;
        sliderAfter.value = lvl;

        SetLevelColor(levelsBefore, request.level);
        SetLevelColor(levelsAfter, lvl);

        requestIcon.sprite = request.icon;
        bubbleIcon.sprite = request.icon;

        CalculateConditions();

    }

    private void SetLevelColor(Transform parent, int id)
    {
        foreach(Transform child in parent)
        {
            TextMeshProUGUI TXT_Level = child.GetComponent<TextMeshProUGUI>();
            TXT_Level.color = Color.black;
        }

        parent.GetChild(id).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
    }

    private void CalculateConditions()
    {
        Tyrannus.CleanParent(PNL_DenyConditions);
        Tyrannus.CleanParent(PNL_AcceptConditions);

        List<SociaCategory> affectedSocialCategories = happinessManager.GetAffectedCategories(dailyRequests[actualCitizen]);

        foreach(SociaCategory socialCategory in affectedSocialCategories)
        {
            GameObject category = Instantiate(scPrefab, PNL_DenyConditions);
            category.GetComponent<SCHappiness>().InitializeSelf(socialCategory);     
        }

        foreach (SociaCategory socialCategory in affectedSocialCategories)
        {
            GameObject category = Instantiate(scPrefab, PNL_AcceptConditions);
            category.GetComponent<SCHappiness>().InitializeSelf(socialCategory, dailyRequests[actualCitizen]);
        }
    }

    #endregion

    #region Decision Section

    public void NextCitizen()                                   //Se cambia de ciudadano
    {
        actualCitizen++;
        if(actualCitizen == numberOfRequests)
        {
            BTN_Citizen.gameObject.SetActive(false);
        }
        else
        {
            ShowCitizen();
        }

        GameManager._instance.moneyManager.CalculateIncoming();

    }

    public void AcceptRequest()
    {
        int sum;

        if (Tyrannus.IsApproval(dailySocialCategories[actualCitizen], dailyRequests[actualCitizen]))
        {
            sum = 1;
        }
        else
        {
            sum = -1;
        }

        dailyRequests[actualCitizen].level += sum;
        happinessManager.CalculateCityHappiness();
        happinessManager.UpdateCitizenHappiness(dailyCitizens[actualCitizen]);
        NextCitizen();

        GameManager._instance.moneyManager.AddTip(tip);
    }

    public void DenyRequest()
    {
        NextCitizen();
    }

    public void MeetCitizen()
    {
        GameManager._instance.contactsManager.AddCitizen(dailyCitizens[actualCitizen]);
    }

    #endregion

}
