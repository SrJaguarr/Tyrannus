using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NeedsLoader : MonoBehaviour
{
    [SerializeField]
    private Sprite sprt_hungry, sprt_cold, sprt_ill;

    [SerializeField]
    private GameObject needPrefab;


    private List<GameObject> needs;

    private void Awake()
    {
        needs = new List<GameObject>();
    }

    public void AddNeed(string need ,Transform parent, string name)
    {
        switch (need)
        {
            case "hungry":
                needPrefab.transform.GetChild(0).GetComponent<Image>().sprite = sprt_hungry;
                break;
            case "cold":
                needPrefab.transform.GetChild(0).GetComponent<Image>().sprite = sprt_cold;
                break;
            case "ill":
                needPrefab.transform.GetChild(0).GetComponent<Image>().sprite = sprt_ill;
                break;
        }
        needPrefab.name = name + "_" + need;
        
        needs.Add(Instantiate(needPrefab, parent));
    }

    private GameObject GetNeed(string name)
    {
        foreach (GameObject n in needs)
        {
            if (n.name.Contains(name))
            {
                return n;
            }
        }

        return null;
    }

    public void RemoveNeed(string name, string need)
    {
        string needString = name + "_" + need;

        GameObject prefab = GetNeed(needString);
        if (prefab != null)
        {
            needs.Remove(prefab);
            Destroy(prefab);
        }
    }

}
