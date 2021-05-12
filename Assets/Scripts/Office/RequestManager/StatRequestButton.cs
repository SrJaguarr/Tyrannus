using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatRequestButton : MonoBehaviour
{

    private Request request;
    bool isApproval;

    [SerializeField]
    private TextMeshProUGUI TXT_Percentage, TXT_SocialCategory;

    [SerializeField]
    private Image IMG_Emoji, IMG_Icon;

    [SerializeField]
    private RectTransform statusBar;

    private Vector2 totalBarSize;


    private void ClickRequest()
    {
        GameManager._instance.canvasManager.ShowRequestViewer(true);
        GameManager._instance.canvasManager.ShowSCategoryStats(false);
        GameManager._instance.requestStats.ShowAffectedCategories(request);
    }

    public void InitializeSelf(Request r, bool b)
    {
        totalBarSize = statusBar.sizeDelta;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { ClickRequest(); });

        request = r;
        isApproval = b;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        TXT_SocialCategory.text = request.requestName;
        TXT_Percentage.text = "NV." + request.level.ToString();

        Vector2 size = totalBarSize;
        size.y = (float)request.level / 5 * totalBarSize.y;

        if(size.y < totalBarSize.y * 0.1)
        {
            size.y = totalBarSize.y * 0.1f;
        }

        statusBar.sizeDelta = size;

        IMG_Emoji.sprite = Tyrannus.GetCorrectEmoji(isApproval, request.level);
        IMG_Icon.sprite = request.icon;
    }


}
