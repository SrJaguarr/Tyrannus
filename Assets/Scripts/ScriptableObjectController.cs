using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectController : MonoBehaviour
{
    private List<Familiar> familiars;
    private List<Request> requests;
    private List<Loan> loans;
    private List<Notification> notifications;

    private Request[] requestsToSave;
    private Familiar[] familiarsToSave;
    private Loan[] loansToSave;
    private Notification[] notificationsToSave;

    private void Start()
    {
        familiars = new List<Familiar>();
        requests = new List<Request>();
        loans = new List<Loan>();
        notifications = new List<Notification>();

        requestsToSave = GameManager._instance.requestDB.requests;
        familiarsToSave = GameManager._instance.familyDB.familiars;
        loansToSave = GameManager._instance.loanDB.loans;
        notificationsToSave = GameManager._instance.notificationDB.notifications;

        SaveFamiliars();
        SaveRequests();
        SaveLoans();
        SaveNotifications();

    }

    private void SaveNotifications()
    {
        for (int i = 0; i < notificationsToSave.Length; i++)
        {
            notifications.Add(Instantiate(notificationsToSave[i]));
        }
    }

    private void SaveFamiliars()
    {
        for (int i = 0; i < familiarsToSave.Length; i++)
        {
            familiars.Add(Instantiate(familiarsToSave[i]));
        }
    }

    private void SaveRequests()
    {
        for (int i = 0; i < requestsToSave.Length; i++)
        {
            requests.Add(Instantiate(requestsToSave[i]));
        }
    }

    private void SaveLoans()
    {
        for (int i = 0; i < loansToSave.Length; i++)
        {
            loans.Add(Instantiate(loansToSave[i]));
        }
    }

    private void RestoreFamiliars()
    {
        for (int i = 0; i < familiarsToSave.Length; i++)
        {
            familiarsToSave[i].daysCold = familiars[i].daysCold;
            familiarsToSave[i].daysHungry = familiars[i].daysHungry;
            familiarsToSave[i].daysIll = familiars[i].daysIll;
            familiarsToSave[i].isCold = familiars[i].isCold;
            familiarsToSave[i].isIll = familiars[i].isIll;
            familiarsToSave[i].isHungry = familiars[i].isHungry;
            familiarsToSave[i].illProbability = familiars[i].illProbability;
            familiarsToSave[i].enjoyedCompany = familiars[i].enjoyedCompany;
            familiarsToSave[i].schoolPenalty = familiars[i].schoolPenalty;

            familiarsToSave[i].feed = familiars[i].feed;
            familiarsToSave[i].heal = familiars[i].heal;
            familiarsToSave[i].heat = familiars[i].heat;
        }
    }

    private void RestoreRequests()
    {
        for (int i = 0; i < requestsToSave.Length; i++)
        {
            requestsToSave[i].level = requests[i].level;
        }
    }

    private void RestoreLoans()
    {
        for (int i = 0; i < loansToSave.Length; i++)
        {
            loansToSave[i].totalAmount = loans[i].totalAmount;
            loansToSave[i].totalInterest = loans[i].totalInterest;
            loansToSave[i].paidAmount = loans[i].paidAmount;
            loansToSave[i].passedDays = loans[i].passedDays;
            loansToSave[i].isActive = loans[i].isActive;
        }
    }

    private void RestoreNotifications()
    {
        for (int i = 0; i < notificationsToSave.Length; i++)
        {
            notificationsToSave[i].hasAppeared = notifications[i].hasAppeared;
        }
    }

    private void OnApplicationQuit()
    {
        RestoreValues();
    }

    public void RestoreValues()
    {
        RestoreFamiliars();
        RestoreRequests();
        RestoreLoans();
        RestoreNotifications();
    }
}
