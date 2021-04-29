using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("Managers & Controllers")]
    public MoneyManager moneyManager;
    public HappinessManager happinessManager;
    public CanvasManager canvasManager;
    public LoanManager loanManager;
    public ShopListController shopListController;
    public CitizenRequest citizenRequest;
    public FamilyController familyController;
    public NeedsLoader needsLoader;
    public ContactsManager contactsManager;
    public NotificationManager notificationManager;
    public TimeManager timeManager;
    public StatsViewerManager statsViewerManager;
    public RequestStats requestStats;
    public ScriptableObjectController scriptableObjectController;
    public FamilyHappiness familyHappiness;

    [Header("Databases")]
    public RequestDB requestDB;
    public SocialCategoryDB socialCategoryDB;
    public FamilyDB familyDB;
    public CitizenDB citizenDB;
    public LoanDB loanDB;
    public EmojiDB emojiDB;
    public NotificationDB notificationDB;
    public Familiar president;

    public int negativeStateMoneyCount;
    public int currentDay = 1;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void NewGame()
    {
        loanManager.NewGame();
        scriptableObjectController.RestoreValues();
        moneyManager.NewGame();
        timeManager.NewGame();
        contactsManager.NewGame();
        shopListController.NewGame();
        familyController.NewGame();
        happinessManager.NewGame();
    }

    public void PauseGame()
    {
        timeManager.pauseTime = !timeManager.pauseTime;
    }

    public void NextDay()
    {
        loanManager.PassDay();
        CheckStateMoney();

        moneyManager.ResetTips();
        citizenRequest.CitizenSelector();

        happinessManager.CalculateGlobalHappiness();
        moneyManager.CalculateSalary(happinessManager.cityHappiness);
        moneyManager.SetStateMoney();

        shopListController.UpdateSalary();
        shopListController.ClearBoughtNeeds();

        familyController.UpdateNeeds();


    }

    private void CheckStateMoney()
    {
        if(moneyManager.stateMoney < 0)
        {
            negativeStateMoneyCount++;

            switch (negativeStateMoneyCount)
            {
                case 1:
                    happinessManager.SetHandicap(0.25f);
                    break;
                case 2:
                    happinessManager.SetHandicap(0.5f);
                    break;
                case 3:
                    happinessManager.SetHandicap(0.75f);
                    break;
                default:
                    happinessManager.SetHandicap(1);
                    break;
            }
        }
        else
        {
            negativeStateMoneyCount = 0;
            happinessManager.SetHandicap(0);
        }

        happinessManager.ClearPenalties();
    }


}
