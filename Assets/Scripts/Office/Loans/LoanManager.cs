using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanManager : MonoBehaviour
{
    private Loan[] loans;
    [SerializeField] Transform loanLayout;
    [SerializeField] GameObject loanPrefab;

    private Dictionary<Loan, GameObject> loanPrefabs;
    private MoneyManager moneyManager;

    private void Start()
    {
        moneyManager = GameManager._instance.moneyManager;

        loans = GameManager._instance.loanDB.loans;
        loanPrefabs = new Dictionary<Loan, GameObject>();

        LoadLoans();
    }

    public void StartLoan(Loan loan)
    {
        loan.isActive = true;
        moneyManager.ReceiveLoan(loan);
        GameManager._instance.happinessManager.AddLoanPenalty(loan.penalty);
        GameManager._instance.happinessManager.CalculateCityHappiness();
        loan.paidAmount = 0;
        loan.passedDays = 0;
    }

    public bool PayLoan(Loan loan)
    {
        bool res = false;
        int dif = loan.totalAmount - loan.paidAmount;

        if(moneyManager.stateMoney >= dif)
        {
            loan.isActive = false;
            moneyManager.PayLoan(loan, dif);

            res = true;
        }

        return res;
    }

    public void PassDay()
    {
        foreach(KeyValuePair<Loan, GameObject> entry in loanPrefabs)
        {
            if (entry.Key.isActive)
            {
                entry.Key.passedDays++;
                entry.Key.paidAmount += entry.Key.totalAmount / entry.Key.days;

                if (entry.Key.passedDays == entry.Key.days)
                {
                    moneyManager.EndLoan(entry.Key);
                    entry.Key.isActive =false;
                    entry.Value.GetComponent<LoanButton>().PayLoan();
                }

                entry.Value.GetComponent<LoanButton>().UpdateActiveLoan();
            }
        }
    }

    private void LoadLoans()
    {
        for(int i = 0; i < loans.Length; i++)
        {
            loanPrefabs.Add(loans[i], Instantiate(loanPrefab, loanLayout));
            loanPrefabs[loans[i]].GetComponent<LoanButton>().loan = loans[i];
            loanPrefabs[loans[i]].GetComponent<LoanButton>().loanManager = this;
        }
    }

    public void NewGame()
    {
        foreach (KeyValuePair<Loan, GameObject> entry in loanPrefabs)
        {
            entry.Value.GetComponent<LoanButton>().PayLoan();
        }

    }

    
}
