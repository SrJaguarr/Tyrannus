using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI TXT_Title, TXT_Description, TXT_Accept, TXT_Deny;
    [SerializeField] private GameObject PNL_Notification, PNL_Decision, BTN_NoDecision;

    private Notification currentNotification;

    private Notification[] notificationDB;
    private HappinessManager happinessManager;

    private bool answer = false;


    [SerializeField] private int happinessThreshold;

    private void Awake()
    {
        gameManager = GameManager._instance;
        happinessManager = gameManager.happinessManager;
        notificationDB = gameManager.notificationDB.notifications;
    }

    public IEnumerator CheckHappinessThreshold()
    {
        foreach (SociaCategory socialCategory in gameManager.socialCategoryDB.categories)
        {
            GameManager._instance.Pause();

            currentNotification = GetNotificationByID(socialCategory.id);

            if (socialCategory.happiness <= happinessThreshold)
            {
                if (!currentNotification.hasAppeared)
                {
                    ShowNotification();
                    currentNotification.hasAppeared = true;
                    while (!answer)
                    {
                        yield return null;
                    }
                }

                if (socialCategory.id == "socialists")
                    gameManager.expulsionCounter++;

                answer = false;

            }
            else
            {
                if (currentNotification != null && !currentNotification.oneTimeNotification)
                {
                    happinessManager.RemovePenalty(socialCategory.id);
                    currentNotification.hasAppeared = false;

                    switch (currentNotification.id)
                    {
                        case "retireds":
                            gameManager.citizenRequest.canRequest = true;
                            break;
                        case "parents":
                            gameManager.familyController.RemoveSchoolPenalty();
                            break;
                        case "liberals":
                            gameManager.moneyManager.RemovePenalty();
                            break;
                        case "socialists":
                            gameManager.expulsionCounter = 0;
                            break;
                    }
                }
            }

            GameManager._instance.Resume();
        }

        happinessManager.CalculateGlobalHappiness();
        gameManager.citizenRequest.CitizenSelector();
        gameManager.moneyManager.CalculateIncoming();
        gameManager.CheckLoose();
    }

    public void ShowNotification(string id)
    {
        currentNotification = GetNotificationByID(id);
        ShowNotification();
    }


    public void ShowNotification()
    {
        if(currentNotification != null)
        {
            TXT_Title.text = currentNotification.title;
            TXT_Description.text = currentNotification.description;

            PNL_Decision.SetActive(currentNotification.isDecision);
            BTN_NoDecision.SetActive(!currentNotification.isDecision);

            if (currentNotification.isDecision)
            {
                TXT_Accept.text = currentNotification.acceptText;
                TXT_Deny.text = currentNotification.denyText;
            }

            PNL_Notification.SetActive(true);
        }
    }

    public Notification GetNotificationByID(string id)
    {
        Notification notification = null;

        for(int i = 0; i < notificationDB.Length; i++)
        {
            if (notificationDB[i].id.Equals(id))
            {
                return notificationDB[i];
            }
        }


        return notification;
    }

    public void Accept()
    {
        switch (currentNotification.id)
        {
            case "request_confirm":
                gameManager.requestStats.ConfirmChanges();
                gameManager.canvasManager.ShowRequestViewer(false);
                gameManager.requestStats.CleanCategories();
                gameManager.requestStats.CleanRequests();
                break;
            case "ethnic_minorities":
                gameManager.familyController.AddFamiliar(4);
                break;
        }
        GameManager._instance.Resume();
        PNL_Notification.SetActive(false);
        answer = true;
    }

    public void Deny()
    {
        switch (currentNotification.id)
        {
            case "request_confirm":
                gameManager.requestStats.ClearChanges();
                gameManager.canvasManager.ShowRequestViewer(false); 
                gameManager.requestStats.CleanCategories(); 
                gameManager.requestStats.CleanRequests();
                break;
            case "ethnic_minorities":
                break;

        }
        GameManager._instance.Resume();
        PNL_Notification.SetActive(false);
        answer = true;
    }

    public void CloseNotification()
    {
        switch (currentNotification.id)
        {
            case "end_week":
                gameManager.timeManager.NewWeek();
                break;
            case "capitalists": //Add penalización del X% a todas las categorias sociales menos a la suya
                PenaltyAllButMe(currentNotification.id, 0.8f);
                break;
            case "conservatives":
                happinessManager.GetSocialCategoryByID("liberals").happinessPenalty.Add(currentNotification.id, 0.80f);
                happinessManager.GetSocialCategoryByID("state_workers").happinessPenalty.Add(currentNotification.id, 0.80f);
                happinessManager.GetSocialCategoryByID("youth").happinessPenalty.Add(currentNotification.id, 0.80f);
                break;
            case "drivers":
                happinessManager.GetSocialCategoryByID("retireds").happinessPenalty.Add(currentNotification.id, 0.8f);
                happinessManager.GetSocialCategoryByID("capitalists").happinessPenalty.Add(currentNotification.id, 0.6f);
                break;
            case "liberals":
                gameManager.moneyManager.AddMoneyPenalty((100 - happinessManager.GetSocialCategoryByID(currentNotification.id).populationPercentage) / 100.0f);
                break;
            case "patriots":
                happinessManager.GetSocialCategoryByID("ethnic_minorities").happinessPenalty.Add(currentNotification.id, 0.6f);
                break;
            case "poors":
                happinessManager.GetSocialCategoryByID("socialists").happinessPenalty.Add(currentNotification.id, 0.80f);
                happinessManager.GetSocialCategoryByID("religious").happinessPenalty.Add(currentNotification.id, 0.70f);
                happinessManager.GetSocialCategoryByID("liberals").happinessPenalty.Add(currentNotification.id, 0.80f);
                break;
            case "religious":
                happinessManager.GetSocialCategoryByID("poors").happinessPenalty.Add(currentNotification.id, 0.6f);
                break;
            case "retireds":
                gameManager.citizenRequest.canRequest = false;
                PenaltyAllButMe(currentNotification.id, 0.85f);
                break;
            case "state_workers":
                PenaltyAllButMe(currentNotification.id, 0.75f);
                break;
            case "youth":
                PenaltyAllButMe(currentNotification.id, 0.9f);
                break;
            case "socialists":
                PenaltyAllButMe(currentNotification.id, 0.85f);
                break;
            case "parents":
                gameManager.familyController.AddSchoolPenalty(0.5f);
                break;
            case "environmentalists":
                happinessManager.GetSocialCategoryByID("drivers").happinessPenalty.Add(currentNotification.id, 0.6f);
                happinessManager.GetSocialCategoryByID("capitalists").happinessPenalty.Add(currentNotification.id, 0.6f);
                break;
        }

        PNL_Notification.SetActive(false);
        answer = true;
    }

    private void PenaltyAllButMe(string myID, float penalty)
    {
        foreach (SociaCategory sc in gameManager.socialCategoryDB.categories)
        {
            if (sc.id != myID )
                happinessManager.GetSocialCategoryByID(sc.id).happinessPenalty.Add(myID, penalty);
        }
    }
}
