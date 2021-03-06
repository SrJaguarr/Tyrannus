using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/Notification")]
public class Notification : ScriptableObject
{
    public string id;
    public string title;
    [TextArea]
    public string description;
    public string acceptText;
    public string denyText;
    public bool isDecision;
    public bool hasAppeared;
    public bool oneTimeNotification;
}