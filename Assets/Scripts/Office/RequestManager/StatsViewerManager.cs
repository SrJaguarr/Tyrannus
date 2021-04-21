using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsViewerManager : MonoBehaviour
{
    private SocialCategoryDB socialCategoryDB;
    
    [SerializeField] 
    private GameObject SCPrefab;

    [SerializeField]
    private Transform PNL_Layout;

    private List<StatButton> socialCategoriesStats;

    private void Awake()
    {
        socialCategoriesStats = new List<StatButton>();
    }

    private void Start()
    {
        socialCategoryDB = GameManager._instance.socialCategoryDB;
        InitializeSocialCategories();
    }

    private void InitializeSocialCategories()
    {
        for(int i = 0; i < socialCategoryDB.categories.Length; i++)
        {
            GameObject button = Instantiate(SCPrefab, PNL_Layout);
            button.GetComponent<StatButton>().InitializeSelf(socialCategoryDB.categories[i]);
            socialCategoriesStats.Add(button.GetComponent<StatButton>());
        }
    }

    public void UpdateStats()
    {
        foreach(StatButton category in socialCategoriesStats)
        {
            category.UpdateInfo();
        }
    }


}
