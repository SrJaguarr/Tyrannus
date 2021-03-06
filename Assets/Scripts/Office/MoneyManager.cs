using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int stateMoney;

    [SerializeField]
    private int maxSalary;

    [SerializeField]
    private TextMeshProUGUI TXT_StateMoney, TXT_IncomingMoney;

    [SerializeField] private Image IMG_WarningIcon;

    public int salary;
    public int incoming;
    private int incomingLoans;
    private List<Loan> activeLoans;
    private int tips;
    private float penalty;

    [SerializeField] private Sprite yellowWarning, redWarning;

    private void Awake()
    {
        activeLoans = new List<Loan>();
    }

    public void NewGame()
    {
        ResetTips();
        activeLoans.Clear();
        stateMoney = 0;
        ShowStateMoney();
        CalculateIncoming();
    }

    public void CalculateIncoming()
    {
        CalculateRequestsIncoming();
        CalculateLoansIncoming();

        ShowIncomingMoney();
    }

    private void CalculateRequestsIncoming()
    {
        Request[] requests = GameManager._instance.requestDB.requests;
        int money = 0;

        for (int i = 0; i < requests.Length; i++)
        {
            if (requests[i].costPerLevel.Length > 0)
                money -= requests[i].costPerLevel[requests[i].level];
        }

        if (penalty > 0)
            money = (int)(incoming * penalty);

        incoming = money;
    }

    private void CalculateLoansIncoming()
    {
        incomingLoans = 0;

        for (int i = 0; i < activeLoans.Count; i++)
        {
            if (activeLoans[i].paidAmount + (activeLoans[i].totalAmount / activeLoans[i].days) <= activeLoans[i].totalAmount)
            {
                incomingLoans += activeLoans[i].totalAmount / activeLoans[i].days;
            }
            else
            {
                incomingLoans += activeLoans[i].totalAmount - activeLoans[i].paidAmount;
            }
        }

        incoming -= incomingLoans;
    }

    public void AddMoneyPenalty(float p)
    {
        penalty = p;
    }

    public void RemovePenalty()
    {
        penalty = 0;
    }

    public void CalculateSalary(int globalHappiness)
    {
        float multiplier = (float)globalHappiness / 100.0f;
        salary += (int)(maxSalary * multiplier) + tips;
    }

    private void ShowStateMoney()
    {
        SetTextColor(stateMoney, TXT_StateMoney);

        TXT_StateMoney.text = stateMoney.ToString() + "€";

        CheckWarning();
    }
    private void ShowIncomingMoney()
    {
        SetTextColor(incoming, TXT_IncomingMoney);
        TXT_IncomingMoney.text = "(" + incoming.ToString() + "€" + ")";
    }

    public void SetStateMoney()
    {
        stateMoney += incoming;

        ShowStateMoney();
    }

    private void CheckWarning()
    {
        IMG_WarningIcon.gameObject.SetActive(stateMoney < 0);

        if(GameManager._instance.negativeStateMoneyCount >= 2)
        {
            IMG_WarningIcon.sprite = redWarning;
        }
        else
        {
            IMG_WarningIcon.sprite = yellowWarning;
        }
    }

    private void SetTextColor(int money, TextMeshProUGUI text)
    {
        if (money < 0)
        {

            text.color = "B52F30".ToColor(); //RED
        }
        else
        {
            text.color = "2C7556".ToColor(); //GREEN
        }
    }

    public void Shop(int cost)
    {
        salary -= cost;
    }

    public void ReceiveLoan(Loan loan)
    {
        stateMoney += loan.amount;

        activeLoans.Add(loan);
        CalculateIncoming();

        ShowIncomingMoney();
        ShowStateMoney();
    }

    public void PayLoan(Loan loan, int n)
    {
        stateMoney -= n;
        activeLoans.Remove(loan);

        CalculateIncoming();
        ShowIncomingMoney();
        ShowStateMoney();
    }

    public void EndLoan(Loan loan)
    {
        activeLoans.Remove(loan);

        CalculateIncoming();
        ShowIncomingMoney();
        ShowStateMoney();
    }

    public void AddTip(int n)
    {
        tips += n;
    }

    public void ResetTips()
    {
        tips = 0;
    }
}
