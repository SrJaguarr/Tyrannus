using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/President")]
public class President : ScriptableObject
{
    public int happiness = 100;

    public bool isHungry = false;
    public int daysHungry = 0;
    public int maxDaysHungry;

    public bool isCold = false;
    public int daysCold = 0;
    public int maxDaysCold;

    public float illProbability = 0.1f;
    public bool isIll = false;
    public int daysIll = 0;
    public int maxDaysIll;

    public bool feed;
    public bool heat;
    public bool heal;

    public Sprite avatarHead;
}
