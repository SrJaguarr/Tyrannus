using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/Citizen")]
public class Citizen : ScriptableObject
{
    public string fullName;

    [TextArea(0, 300)] public string description;

    public Sprite sprite;
    public SociaCategory[] socialCategories;
    public Sprite spriteHead;
    public int age;
    public int citizenHappiness;
    public bool genre; //true = male // false = female
}
