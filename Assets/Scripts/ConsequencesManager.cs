using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsequencesManager : MonoBehaviour
{
    private SocialCategoryDB categories;
    private NotificationManager notificationManager;
    [SerializeField] private int happinessThreshold;

    private void Start()
    {
        categories = GameManager._instance.socialCategoryDB;
        notificationManager = GameManager._instance.notificationManager;
    }

    public void CheckHappinessThreshold()
    {
        foreach (SociaCategory socialCategory in categories.categories)
        {
            if(socialCategory.happiness <= happinessThreshold)
            {
                StartCoroutine(notificationManager.ShowNotification(socialCategory.id));

                GameManager._instance.Pause();
            }
        }
    }
}
