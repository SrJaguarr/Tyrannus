using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public bool doingTutorial;
    bool firstGame;
    private int currentDay;

    private GameManager gameManager;
    private NotificationManager notificationManager;

    [SerializeField] GameObject IMG_Cot;

    private void Awake()
    {
        gameManager = GameManager._instance;
        notificationManager = gameManager.notificationManager;
        currentDay = 0;
        firstGame = true;
    }

    public void DoTutorial(bool b)
    {
        doingTutorial = b;
        firstGame = false;

        if (b)
        {
            StartTutorial();
        }
        else
        {
            SkipTutorial();
        }

    }

    public void NewGame()
    {
        currentDay = gameManager.timeManager.day;

        if (firstGame || doingTutorial)
        {
            notificationManager.ShowNotification("tutorial");
        }
        else
        {
            gameManager.timeManager.enabled = true;
            SkipTutorial();
        }
    }

    private void StartTutorial()
    {
        currentDay = gameManager.timeManager.day;
        ManageDay();
    }

    private void SkipTutorial()
    {
        gameManager.familyController.GetFamiliarButton(0).GetComponent<Button>().interactable = true;
        gameManager.canvasManager.ShowHappinessInfo();
        gameManager.canvasManager.ShowTimer();
        gameManager.canvasManager.BTN_LeaveHome.gameObject.GetComponent<Button>().interactable = true;
        IMG_Cot.SetActive(true);
        gameManager.familyController.AddFamiliar(1);
        gameManager.familyController.GetFamiliarButton(1).GetComponent<Button>().interactable = true;
        gameManager.familyController.AddFamiliar(2);
        gameManager.familyController.AddFamiliar(3);
    }

    public void ManageDay()
    {
        currentDay = gameManager.timeManager.day;
        switch(currentDay)
        {
            case 1:
                notificationManager.ShowNotification("tuto_day1");

                gameManager.moneyManager.salary = 25;
                gameManager.shopListController.UpdateSalary();

                gameManager.familyController.GetFamiliar(0).illProbability = 0;
                gameManager.familyController.GetFamiliar(0).isCold = false;
                break;
            case 2:
                gameManager.familyController.AddFamiliar(1);

                notificationManager.ShowNotification("tuto_day2");

                gameManager.moneyManager.salary = 30;
                gameManager.shopListController.UpdateSalary();

                gameManager.familyController.GetFamiliar(0).illProbability = 0;
                gameManager.familyController.GetFamiliar(1).illProbability = 0;
                gameManager.familyController.GetFamiliar(0).isCold = false;
                gameManager.familyController.GetFamiliar(1).isCold = false;

                gameManager.familyController.UpdateNeeds();

                break;
            case 3:
                notificationManager.ShowNotification("tuto_day3");

                gameManager.moneyManager.salary = 45;
                gameManager.shopListController.UpdateSalary();

                gameManager.familyController.GetFamiliar(1).illProbability = 1;
                break;
            case 4:

                gameManager.moneyManager.salary = 55;
                gameManager.shopListController.UpdateSalary();

                gameManager.familyController.GetFamiliar(0).illProbability = 0.05f;
                gameManager.familyController.GetFamiliar(1).illProbability = 0.05f;
                notificationManager.ShowNotification("tuto_day4");
                break;
            case 5:
                gameManager.moneyManager.salary = 55;
                gameManager.shopListController.UpdateSalary();
                notificationManager.ShowNotification("tuto_day5");
                gameManager.familyController.GetFamiliarButton(1).GetComponent<Button>().interactable = true;
                break;
            case 6:
                gameManager.moneyManager.salary = 55;
                gameManager.shopListController.UpdateSalary();
                notificationManager.ShowNotification("tuto_day6");
                break;
            case 7:
                gameManager.moneyManager.salary = 55;
                gameManager.shopListController.UpdateSalary();
                notificationManager.ShowNotification("tuto_day7");
                break;
            case 8:
                gameManager.canvasManager.ShowOffice();
                gameManager.canvasManager.BTN_LeaveHome.gameObject.GetComponent<Button>().interactable = true;
                notificationManager.ShowNotification("tuto_day8");
                break;
            case 9:
                gameManager.familyController.GetFamiliarButton(0).GetComponent<Button>().interactable = true;
                notificationManager.ShowNotification("tuto_day9");
                gameManager.canvasManager.ShowHappinessInfo();
                break;
            case 10:
                gameManager.canvasManager.ShowOffice();
                notificationManager.ShowNotification("tuto_day10");
                break;
            case 12:
                gameManager.canvasManager.ShowOffice();
                notificationManager.ShowNotification("tuto_day12");
                break;
            case 15:
                gameManager.familyController.AddFamiliar(2);
                notificationManager.ShowNotification("tuto_day15");
                break;
            case 18:
                IMG_Cot.SetActive(true);
                break;
            case 22:
                gameManager.familyController.AddFamiliar(3);
                notificationManager.ShowNotification("tuto_day22");
                doingTutorial = false;
                break;
            default:
                gameManager.Resume();
                break;
        }
    }
}
