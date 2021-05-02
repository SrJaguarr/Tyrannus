using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsequencesManager : MonoBehaviour
{
    private SocialCategoryDB categories;
    private NotificationManager notificationManager;

    private void Start()
    {
        categories = GameManager._instance.socialCategoryDB;
        notificationManager = GameManager._instance.notificationManager;
    }
}
