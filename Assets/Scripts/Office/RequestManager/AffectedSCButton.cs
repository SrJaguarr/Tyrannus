using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AffectedSCButton : MonoBehaviour
{

    private SociaCategory socialCategory;
    private bool isApproval;
    private int level;

    [SerializeField]
    private TextMeshProUGUI TXT_Percentage, TXT_SocialCategory;

    [SerializeField]
    private Image IMG_Emoji;

    [SerializeField]
    private RectTransform statusBar;

    private Vector2 totalBarSize;


    private void ClickStat()
    {
        GameManager._instance.requestStats.ClearChanges();
        GameManager._instance.requestStats.CleanCategories();
        GameManager._instance.requestStats.CleanRequests();

        GameManager._instance.canvasManager.ShowSCategoryStats(true);
        GameManager._instance.canvasManager.ShowRequestViewer(false);
        GameManager._instance.requestStats.ShowRequestList(socialCategory);
    }

    public void InitializeSelf(SociaCategory sc, Request r)
    {
        totalBarSize = statusBar.sizeDelta;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { ClickStat(); });

        socialCategory = sc;
        isApproval = Tyrannus.IsApproval(sc, r);
        level = r.level;

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        TXT_SocialCategory.text = socialCategory.categoryName;

        Vector2 size = totalBarSize;

        float multiplier = ((float)level) / 4;

        if (!isApproval)
        {
            multiplier = Mathf.Abs((1f - multiplier));
        }

        size.y = multiplier * totalBarSize.y;

        if (size.y < totalBarSize.y * 0.15)
        {
            size.y = totalBarSize.y * 0.15f;
        }
        statusBar.sizeDelta = size;

        TXT_Percentage.text = (multiplier * 100).ToString()  + "%";

        IMG_Emoji.sprite = Tyrannus.GetCorrectEmoji(isApproval, level);
    }

    public void UpdateLevel(int n)
    {
        level = n;
        UpdateInfo();
    }

}
