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

    private bool answer = false;

    private SocialCategoryDB categories;
    [SerializeField] private int happinessThreshold;

    private void Awake()
    {
        gameManager = GameManager._instance;
        categories = gameManager.socialCategoryDB;
        notificationDB = gameManager.notificationDB.notifications;
    }

    public IEnumerator CheckHappinessThreshold()
    {
        foreach (SociaCategory socialCategory in categories.categories)
        {
            GameManager._instance.Pause();
            if (socialCategory.happiness <= happinessThreshold)
            {
                ShowNotification(socialCategory.id);

                while (!answer)
                {
                    yield return null;
                }

                answer = false;
            }
            GameManager._instance.Resume();
        }
    }

    public void ShowNotification(string notificationID)
    {
        currentNotification = GetNotificationByID(notificationID);

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

    private Notification GetNotificationByID(string id)
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
        }

        PNL_Notification.SetActive(false);
        answer = true;
    }
}
