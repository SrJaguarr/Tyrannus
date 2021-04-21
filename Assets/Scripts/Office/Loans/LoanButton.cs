using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoanButton : MonoBehaviour
{
    public Loan loan;
    public LoanManager loanManager;

    [SerializeField] GameObject PNL_Active, PNL_Inactive;
    [SerializeField] Button BTN_Pay, BTN_Start;
    [SerializeField] TextMeshProUGUI TXT_Amount, TXT_InterestPercentage, TXT_PendingAmount, TXT_Days, TXT_TotalInterest, TXT_PendingDays;
    [SerializeField] RectTransform progressBar;

    private float totalBarSize;

    private void Start()
    {
        BTN_Pay.onClick.AddListener(delegate { PayLoan(); });
        BTN_Start.onClick.AddListener(delegate { StartLoan(); });

        totalBarSize = progressBar.sizeDelta.x;

        loan.totalAmount = loan.amount + (loan.amount * loan.interest / 100);
        loan.totalInterest = loan.amount * loan.interest / 100;
        CustomizeLoan();
    }

    private void CustomizeLoan()
    {

        if (loan.isActive)
        {
            UpdateActiveLoan();
        }
        else
        {
            UpdateInactiveLoan();
        }

        TXT_Amount.text = loan.amount.ToString("0,0").Replace(',', '.') + "€";
        TXT_InterestPercentage.text = loan.interest.ToString() + "% INTERES";

        UpdateLayouts();
    }

    public void UpdateActiveLoan()
    {
        TXT_PendingAmount.text = (loan.totalAmount - loan.paidAmount).ToString() + " € POR PAGAR";
        TXT_PendingDays.text = (loan.days - loan.passedDays).ToString() + " DIAS RESTANTES";
        UpdateProgressBar();
    }

    private void UpdateInactiveLoan()
    {
        TXT_TotalInterest.text = loan.totalInterest.ToString() + "€";
        TXT_Days.text = loan.days.ToString() + " DIAS";
    }

    private void UpdateProgressBar()
    {
        Vector2 size = progressBar.sizeDelta;
        size.x = ((float) loan.paidAmount / loan.totalAmount) * totalBarSize;
        progressBar.sizeDelta = size;
    }

    public void StartLoan()
    {
        loanManager.StartLoan(loan);
        UpdateLayouts();
        UpdateActiveLoan();
    }

    public void PayLoan()
    {
        if (loanManager.PayLoan(loan))
        {
            UpdateLayouts();
            UpdateInactiveLoan();
        }
    }

    private void UpdateLayouts()
    {
        PNL_Inactive.SetActive(!loan.isActive);
        PNL_Active.SetActive(loan.isActive);
    }
}
