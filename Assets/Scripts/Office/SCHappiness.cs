using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCHappiness : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TXT_Happiness, TXT_Category;

    [SerializeField]
    private Image emoji;

    public void InitializeSelf(SociaCategory sc, Request request)
    {
        TXT_Category.text = sc.categoryName;
        TXT_Happiness.text = sc.happiness.ToString() + "%";
        SetEmoji(sc.happiness);

        request.level++;
        GameManager._instance.happinessManager.CalculateSocialCategoryHappines(sc);
        TXT_Happiness.text = sc.happiness.ToString() + "%";
        SetEmoji(sc.happiness);

        request.level--;
        GameManager._instance.happinessManager.CalculateSocialCategoryHappines(sc);
    }

    public void InitializeSelf(SociaCategory sc)
    {
        TXT_Category.text = sc.categoryName;
        TXT_Happiness.text = sc.happiness.ToString() + "%";
        SetEmoji(sc.happiness);
    }

    private void SetEmoji(int happiness)
    {
        emoji.sprite = Tyrannus.GetCorrectEmoji(happiness);
    }

}
