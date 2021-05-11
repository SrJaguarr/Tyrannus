using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    bool firstGame;
    private int currentDay;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager._instance;
        currentDay = 0;
        firstGame = true;
    }

    public void DoTutorial(bool b)
    {
        firstGame = false;

        if (b)
        {
            StartTutorial();
        }
        else
        {
            SkipTutorial();
        }

        gameManager.timeManager.enabled = true;
    }

    public void NewGame()
    {
        if (firstGame)
        {
            gameManager.notificationManager.ShowNotification("tutorial");
        }
        else
        {
            gameManager.timeManager.enabled = true;
        }
    }

    private void StartTutorial()
    {
        print("HACEMOS TUTO");
    }

    private void SkipTutorial()
    {
        print("NO HACEMOS TUTO");
    }
}
