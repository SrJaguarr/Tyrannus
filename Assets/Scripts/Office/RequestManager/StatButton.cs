using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{

    public SociaCategory socialCategory;

    [SerializeField]
    private TextMeshProUGUI TXT_Percentage, TXT_SocialCategory;

    [SerializeField]
    private Image IMG_Emoji;

    [SerializeField]
    private RectTransform statusBar;

    private Vector2 totalBarSize;


    private void ClickStat()
    {
        GameManager._instance.canvasManager.ShowSCategoryStats(true);
        GameManager._instance.canvasManager.ShowSCategoryViewer(false);
        GameManager._instance.requestStats.ShowRequestList(socialCategory);
    }

    public void InitializeSelf(SociaCategory sc)
    {
        totalBarSize = statusBar.sizeDelta;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { ClickStat(); });

        socialCategory = sc;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        TXT_SocialCategory.text = socialCategory.categoryName;
        TXT_Percentage.text = socialCategory.happiness.ToString() + "%";

        Vector2 size = totalBarSize;
        size.y = (float)socialCategory.happiness / 100 * totalBarSize.y;

        if(size.y < totalBarSize.y * 0.1f)
        {
            size.y = totalBarSize.y * 0.1f;
        }

        statusBar.sizeDelta = size;

        UpdateEmoji();

    }

    private void UpdateEmoji()
    {
        switch (socialCategory.happiness)
        {
            case int n when (n >= 0 && n <= 33):
                IMG_Emoji.sprite =  GameManager._instance.emojiDB.emojis[4].sprite;
                break;
            case int n when (n > 33 && n <= 40):
                IMG_Emoji.sprite = GameManager._instance.emojiDB.emojis[3].sprite;
                break;
            case int n when (n > 40 && n <= 50):
                IMG_Emoji.sprite = GameManager._instance.emojiDB.emojis[2].sprite;
                break;
            case int n when (n > 50 && n <= 60):
                IMG_Emoji.sprite = GameManager._instance.emojiDB.emojis[1].sprite;
                break;
            case int n when (n > 60 && n <= 100):
                IMG_Emoji.sprite = GameManager._instance.emojiDB.emojis[0].sprite;
                break;
        }
    }

}
