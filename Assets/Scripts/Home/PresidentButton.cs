using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresidentButton : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { GameManager._instance.canvasManager.HandleHappinessPanel(); GameManager._instance.familyHappiness.UpdatePresidentInfo(); });
    }
}
