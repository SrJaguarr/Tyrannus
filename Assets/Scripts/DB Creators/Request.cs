using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/Request")]
public class Request : ScriptableObject
{
    public string requestName;
    public string description;
    public Sprite icon;

    public int    level = 3; // 1: none   ||  2: low  ||  3: medium  || 4: high  || 5: maximum

    public int[] costPerLevel;

    public string[] approvalRequest;
    public string[] abolitionRequest;
}
