﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private enum StateMachine { MainMenu, PauseMenu, Game };
    private StateMachine currentState = StateMachine.MainMenu;

    [Header("Panels")]
    [SerializeField] private GameObject PNL_Office;
    [SerializeField] private GameObject PNL_Home;
    [SerializeField] private GameObject PNL_Request;
    [SerializeField] private GameObject PNL_RequestManager;
    [SerializeField] private GameObject PNL_Loans;
    [SerializeField] private GameObject PNL_ShopList;
    [SerializeField] private GameObject PNL_Contacts;
    [SerializeField] private GameObject PNL_SocialCategoryViewer;
    [SerializeField] private GameObject PNL_SocialCategoryStats;
    [SerializeField] private GameObject PNL_RequestViewer;
    [SerializeField] private GameObject PNL_MainMenu_Continue;
    [SerializeField] private GameObject PNL_MainMenu_NewGame;
    [SerializeField] private GameObject PNL_PauseMenu;
    [SerializeField] private GameObject PNL_Credits;
    [SerializeField] private GameObject PNL_GameOver;
    [SerializeField] private GameObject PNL_Familiar;
    [SerializeField] private GameObject PNL_Happiness;

[Header("Main Menu")]
    [SerializeField] private Button BTN_MMC_NewGame;
    [SerializeField] private Button BTN_MMC_Continue;
    [SerializeField] private Button BTN_MMC_Credits;
    [SerializeField] private Button BTN_MMC_Exit;
    [SerializeField] private Button BTN_MMNG_NewGame;
    [SerializeField] private Button BTN_MMNG_Credits;
    [SerializeField] private Button BTN_MMNG_Exit;

    [SerializeField] private Button BTN_BackCredits;

    [Header("Pause Menu")]
    [SerializeField] private Button BTN_Resume;
    [SerializeField] private Button BTN_Resume2;
    [SerializeField] private Button BTN_PauseExit;
    [SerializeField] private Button BTN_Options;

    [Header("Game Over")]
    [SerializeField] private Button BTN_EndGame;
    [SerializeField] private Button BTN_Restart;

    [Header("Citizen Request")]
    [SerializeField] private Button BTN_CitizenRequest;
    [SerializeField] private Button BTN_CloseCitizenRequest;
    [SerializeField] private Button BTN_AcceptCitizenRequest;
    [SerializeField] private Button BTN_DenyCitizenRequest;

    [Header("Familiar View")]
    [SerializeField] private Button BTN_CloseFamiliar;

    [Header("Request Manager")]
    [SerializeField] private Button BTN_RequestManager;
    [SerializeField] private Button BTN_CloseSCViewer;
    [SerializeField] private Button BTN_CloseSCStats;
    [SerializeField] private Button BTN_CloseRequestViewer;
    [SerializeField] private Button BTN_ConfirmChanges;
    [SerializeField] private Button BTN_ClearChanges;
    [SerializeField] private Button BTN_BackSCViewer;
    [SerializeField] private Button BTN_BackSCStats;


    [Header("Shop List")]
    [SerializeField] private Button BTN_OpenShopList;
    [SerializeField] private Button BTN_CloseShopList;
    [SerializeField] private Button BTN_ConfirmShopList;

    [Header("Loans")]
    [SerializeField] private Button BTN_Loans;
    [SerializeField] private Button BTN_CloseLoans;

    [Header("Scene")]
    [SerializeField] private Button BTN_EndOfficeDay;
    [SerializeField] private Button BTN_LeaveHome;

    [Header("Contacts")]
    [SerializeField] private Button BTN_OpenContacts;
    [SerializeField] private Button BTN_CloseContacts;
    [SerializeField] private Button BTN_NextContact;
    [SerializeField] private Button BTN_PreviousContact;

    [Header("Notification")]
    [SerializeField] private Button BTN_NotificationAccept;
    [SerializeField] private Button BTN_NotificationDeny;
    [SerializeField] private Button BTN_NotificationClose;


    [Header("Happiness")]
    [SerializeField] private Button BTN_OpenHappinessPanel;
    [SerializeField] private Button BTN_CloseHappinessPanel;


    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager._instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(currentState == StateMachine.MainMenu))
        {
            HandlePauseScreen();
        }
    }

    private void Start()
    {
        BTN_EndGame.onClick.AddListener(delegate { HandleGameOver(); HandleNewGameMainMenuScreen(); });
        BTN_Restart.onClick.AddListener(delegate { HandleGameOver(); GameManager._instance.NewGame(); });

        BTN_OpenHappinessPanel.onClick.AddListener(delegate { HandleHappinessPanel(); GameManager._instance.familyHappiness.UpdatePresidentInfo(); });
        BTN_CloseHappinessPanel.onClick.AddListener(delegate { HandleHappinessPanel(); });

        BTN_MMC_Continue.onClick.AddListener(delegate { HandleContinueMainMenuScreen(); currentState = StateMachine.Game; });
        BTN_MMNG_NewGame.onClick.AddListener(delegate { HandleNewGameMainMenuScreen(); gameManager.NewGame(); currentState = StateMachine.Game; });
        BTN_MMC_NewGame.onClick.AddListener(delegate { HandleContinueMainMenuScreen(); gameManager.NewGame(); currentState = StateMachine.Game; });
        BTN_MMC_Credits.onClick.AddListener(delegate { HandleCreditsScreen(); });
        BTN_MMNG_Credits.onClick.AddListener(delegate { HandleCreditsScreen(); });
        BTN_MMC_Exit.onClick.AddListener(delegate { Application.Quit(); });
        BTN_MMNG_Exit.onClick.AddListener(delegate { Application.Quit(); });
        BTN_BackCredits.onClick.AddListener(delegate { HandleCreditsScreen();});

        BTN_Resume.onClick.AddListener(delegate { HandlePauseScreen(); currentState = StateMachine.Game; });
        BTN_Resume2.onClick.AddListener(delegate { HandlePauseScreen(); currentState = StateMachine.Game; });
        BTN_PauseExit.onClick.AddListener(delegate { HandleContinueMainMenuScreen(); HandlePauseScreen(); currentState = StateMachine.MainMenu; });

        BTN_RequestManager.onClick.AddListener(delegate { ShowSCategoryViewer(true); });
        BTN_ConfirmChanges.onClick.AddListener(delegate { gameManager.requestStats.ConfirmChanges(); gameManager.happinessManager.CalculateCityHappiness(); BTN_CloseRequestViewer.onClick.Invoke(); gameManager.statsViewerManager.UpdateStats(); gameManager.moneyManager.CalculateIncoming(); });
        BTN_ClearChanges.onClick.AddListener(delegate { gameManager.requestStats.ClearChanges(); });
        BTN_BackSCViewer.onClick.AddListener(delegate { ShowSCategoryViewer(true); ShowSCategoryStats(false); gameManager.requestStats.CleanRequests(); });
        BTN_BackSCStats.onClick.AddListener(delegate { ShowRequestViewer(false); ShowSCategoryStats(true); gameManager.requestStats.CleanCategories(); });
        BTN_CloseSCViewer.onClick.AddListener(delegate { ShowSCategoryViewer(false); gameManager.requestStats.CleanCategories(); gameManager.requestStats.CleanRequests(); });
        BTN_CloseSCStats.onClick.AddListener(delegate { ShowSCategoryStats(false); gameManager.requestStats.CleanCategories(); gameManager.requestStats.CleanRequests(); });
        BTN_CloseRequestViewer.onClick.AddListener( delegate {

            if (gameManager.requestStats.HasChanges())
            {
                gameManager.notificationManager.ShowNotification("request_confirm");
            }
            else
            {
                ShowRequestViewer(false);
                gameManager.requestStats.CleanCategories();
                gameManager.requestStats.CleanRequests();
            }

        });


        BTN_CitizenRequest.onClick.AddListener(delegate { HandleCitizenRequest(); gameManager.citizenRequest.MeetCitizen(); });
        BTN_CloseCitizenRequest.onClick.AddListener(delegate { HandleCitizenRequest(); });
        BTN_AcceptCitizenRequest.onClick.AddListener(delegate { AcceptCitizenRequest(); gameManager.statsViewerManager.UpdateStats(); });
        BTN_DenyCitizenRequest.onClick.AddListener(delegate { DenyCitizenRequest(); });

        BTN_CloseFamiliar.onClick.AddListener(delegate { HandleFamiliarPanel(); });

        BTN_Loans.onClick.AddListener(delegate { HandleLoans(); });
        BTN_CloseLoans.onClick.AddListener(delegate { HandleLoans(); });

        BTN_LeaveHome.onClick.AddListener(delegate { LeaveHome(); });
        BTN_EndOfficeDay.onClick.AddListener(delegate { LeaveOffice(); });

        BTN_OpenShopList.onClick.AddListener(delegate { HandleShopList(); });
        BTN_CloseShopList.onClick.AddListener(delegate { HandleShopList(); });
        BTN_ConfirmShopList.onClick.AddListener(delegate { gameManager.shopListController.Buy(); HandleShopList(); gameManager.familyController.CheckNeeds();  });

        BTN_OpenContacts.onClick.AddListener(delegate { HandleContacts(); gameManager.contactsManager.OpenMobile(); });
        BTN_CloseContacts.onClick.AddListener(delegate { HandleContacts(); });
        BTN_NextContact.onClick.AddListener(delegate { SwitchCitizen(1); });
        BTN_PreviousContact.onClick.AddListener(delegate { SwitchCitizen(-1); });

        BTN_NotificationAccept.onClick.AddListener(delegate { gameManager.notificationManager.Accept(); });
        BTN_NotificationDeny.onClick.AddListener(delegate { gameManager.notificationManager.Deny(); });
        BTN_NotificationClose.onClick.AddListener(delegate { gameManager.notificationManager.CloseNotification(); });
    }

    #region Menus


    private void HandleContinueMainMenuScreen()
    {
        PNL_MainMenu_Continue.SetActive(!PNL_MainMenu_Continue.activeSelf);
    }

    private void HandleNewGameMainMenuScreen()
    {
        PNL_MainMenu_NewGame.SetActive(!PNL_MainMenu_NewGame.activeSelf);
    }

    private void HandlePauseScreen()
    {
        PNL_PauseMenu.SetActive(!PNL_PauseMenu.activeSelf);
        gameManager.PauseGame();
    }
    private void HandleCreditsScreen()
    {
        PNL_Credits.SetActive(!PNL_Credits.activeSelf);
    }

    public void HandleGameOver()
    {
        PNL_GameOver.SetActive(!PNL_GameOver.activeSelf);
    }


    #endregion

    #region Loans
    private void HandleLoans() { PNL_Loans.SetActive(!PNL_Loans.activeSelf); }

    #endregion
    #region Shop List

    private void HandleShopList() { PNL_ShopList.SetActive(!PNL_ShopList.activeSelf); }

    #endregion
    #region Citizen Request
    private void HandleCitizenRequest() { PNL_Request.SetActive(!PNL_Request.activeSelf); }

    private void AcceptCitizenRequest()
    {
        GameManager._instance.citizenRequest.AcceptRequest();
        HandleCitizenRequest();
    }

    private void DenyCitizenRequest()
    {
        GameManager._instance.citizenRequest.DenyRequest();
        HandleCitizenRequest();
    }

    #endregion
    #region Scene Handler

    private void LeaveHome() 
    {
        HandleScene();
    }

    private void LeaveOffice()
    {
        HandleScene();
    }

    private void HandleScene()
    {
        PNL_Home.SetActive(!PNL_Home.activeSelf);
        PNL_Office.SetActive(!PNL_Office.activeSelf);
    }

    #endregion
    #region Contacts
    private void HandleContacts() { PNL_Contacts.SetActive(!PNL_Contacts.activeSelf); }
    private void SwitchCitizen(int n) { GameManager._instance.contactsManager.SwitchCitizen(n); }
    #endregion
    #region Happiness

    private void HandleHappinessPanel()
    {
        PNL_Happiness.SetActive(!PNL_Happiness.activeSelf);
    }
    public void HandleFamiliarPanel()
    {
        PNL_Familiar.SetActive(!PNL_Familiar.activeSelf);
    }

    #endregion
    #region Request System
    public void ShowSCategoryViewer(bool b)
    {
        PNL_SocialCategoryViewer.SetActive(b);
    }

    public void ShowSCategoryStats(bool b)
    {
        PNL_SocialCategoryStats.SetActive(b);
    }
    public void ShowRequestViewer( bool b)
    {
        PNL_RequestViewer.SetActive(b);
    }

    #endregion

}
