using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/Social Category")]
public class SociaCategory : ScriptableObject
{
    public string id;
    public string categoryName;
    public string categoryDescription;
    public Sprite categorySprite;
    [Range(0, 100)] public int populationPercentage; 
    public Request[] requestsToApprove;
    public Request[] RequestsToAbolish;
    [Range(0, 100)] public int happiness;
}
