using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tyrannus/Loan")]
public class Loan : ScriptableObject
{
    [Header("Loan Info")]
    public int amount;
    public int interest;
    public int days;
    public float penalty; 

    [Header("Ingame Info")]
    public int totalInterest;
    public int totalAmount;
    public int paidAmount;
    public int passedDays;
    public bool isActive;
}
