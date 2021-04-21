using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContactsManager : MonoBehaviour
{
    private List<Citizen> knownCitizens;
    [SerializeField] private TextMeshProUGUI TXT_Name, TXT_Categories, TXT_Description;
    [SerializeField] private Image IMG_Picture, IMG_Emoji;

    private int actualCitizen;

    private void Awake()
    {
        knownCitizens = new List<Citizen>();
    }

    public void NewGame()
    {
        knownCitizens.Clear();
    }

    public void AddCitizen(Citizen citizen)
    {
        if(!knownCitizens.Contains(citizen))
            knownCitizens.Add(citizen);
    }

    public void SwitchCitizen(int n)
    {
        if(knownCitizens.Count > 0)
        {
            actualCitizen += n;

            if (actualCitizen < 0)
               actualCitizen = knownCitizens.Count - 1;
            
            if(actualCitizen >= knownCitizens.Count)
                actualCitizen = 0;

            ShowContact();

        }
    }

    private void ShowContact()
    {
        Citizen citizen = knownCitizens[actualCitizen];
        int emoji = 2;

        IMG_Picture.sprite = citizen.spriteHead;
        TXT_Name.text = citizen.fullName;
        TXT_Description.text = citizen.description;

        TXT_Categories.text = "                      ";
        for (int i = 0; i < citizen.socialCategories.Length; i++)
        {
            TXT_Categories.text = TXT_Categories.text + citizen.socialCategories[i].categoryName;

            if (i < citizen.socialCategories.Length - 1)
                TXT_Categories.text = TXT_Categories.text + ", ";
        }
        TXT_Categories.text = TXT_Categories.text;

        switch (citizen.citizenHappiness)
        {
            case int n when (n >= 0 && n <= 20):
                emoji = 4;
                break;
            case int n when (n > 20 && n <= 40):
                emoji = 3;
                break;
            case int n when (n > 40 && n <= 60):
                emoji = 2;
                break;
            case int n when (n > 60 && n <= 80):
                emoji = 1;
                break;
            case int n when (n > 80 && n <= 100):
                emoji = 0;
                break;
        }
        IMG_Emoji.sprite = GameManager._instance.emojiDB.emojis[emoji].sprite;
    }

    public void OpenMobile()
    {
        if(knownCitizens.Count > 0)
        {
            actualCitizen = 0;
            ShowContact();
        }
    }
}
