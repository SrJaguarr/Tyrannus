using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamiliarButton : MonoBehaviour
{
    private Familiar familiar;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { GameManager._instance.canvasManager.HandleFamiliarPanel(); GameManager._instance.familyHappiness.ShowFamiliar(familiar); });
    }

    public void InitializeSelf(Familiar f)
    {
        familiar = f;
    }
}
