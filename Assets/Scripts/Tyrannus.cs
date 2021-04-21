using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tyrannus
{
    public static SociaCategory[] ShuffleCategories(SociaCategory[] categories)
    {
        SociaCategory temp;

        for (int i = 0; i < categories.Length; i++)
        {
            int rnd = Random.Range(0, categories.Length);
            temp = categories[rnd];
            categories[rnd] = categories[i];
            categories[i] = temp;
        }

        return categories;
    }

    public static Request[] ShuffleRequests(Request[] requests)
    {
        Request temp;

        for (int i = 0; i < requests.Length; i++)
        {
            int rnd = Random.Range(0, requests.Length);
            temp = requests[rnd];
            requests[rnd] = requests[i];
            requests[i] = temp;
        }

        return requests;
    }

    public static Citizen[] ShuffleCitizens(Citizen[] citizens)
    {
        Citizen temp;

        for (int i = 0; i < citizens.Length; i++)
        {
            int rnd = Random.Range(0, citizens.Length);
            temp = citizens[rnd];
            citizens[rnd] = citizens[i];
            citizens[i] = temp;
        }

        return citizens;
    }

    public static bool IsApproval(SociaCategory socialCategory, Request request)
    {
        return System.Array.Exists(socialCategory.requestsToApprove, element => element == request);
    } //Devuelve si la peticion es de aprobacion o de abolicion
    public static bool IsAbolishion(SociaCategory socialCategory, Request request)
    {
        return System.Array.Exists(socialCategory.RequestsToAbolish, element => element == request);
    } //Devuelve si la peticion es de aprobacion o de abolicion

    public static void CleanParent(Transform parent)
    {
        foreach(Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static Sprite GetCorrectEmoji(bool isApproval, int level)
    {
        EmojiDB emojiDB = GameManager._instance.emojiDB;

        switch (level)
        {
            case 0:

                if (isApproval)
                {
                    return emojiDB.emojis[4].sprite;
                }
                else
                {
                    return emojiDB.emojis[0].sprite;
                }

            case 1:

                if (isApproval)
                {
                    return emojiDB.emojis[3].sprite;
                }
                else
                {
                    return emojiDB.emojis[1].sprite;
                }

            case 2:

                return emojiDB.emojis[2].sprite;

            case 3:

                if (isApproval)
                {
                    return emojiDB.emojis[1].sprite;
                }
                else
                {
                    return emojiDB.emojis[3].sprite;
                }

            case 4:

                if (isApproval)
                {
                    return emojiDB.emojis[0].sprite;
                }
                else
                {
                    return emojiDB.emojis[4].sprite;
                }

            default:
                return null;

        }
    }
    public static Sprite GetCorrectEmoji(int happiness)
    {
        int emoji = 2;

        switch (happiness)
        {
            case int n when (n >= 0 && n <= 33):
                emoji = 4;
                break;
            case int n when (n > 33 && n <= 40):
                emoji = 3;
                break;
            case int n when (n > 40 && n <= 50):
                emoji = 2;
                break;
            case int n when (n > 50 && n <= 60):
                emoji = 1;
                break;
            case int n when (n > 60 && n <= 100):
                emoji = 0;
                break;
        }

        return GameManager._instance.emojiDB.emojis[emoji].sprite;
    }
}
