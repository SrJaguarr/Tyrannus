using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Day Time in secs")]
    [SerializeField] private int dayTime;
    [SerializeField] private int weekDays;

    [SerializeField] TextMeshProUGUI TXT_Day, TXT_WeekDay;
    [SerializeField] Image radialBar, radialLayout;

    private float dayRemainingTime;

    private int dayOfWeek;
    public int day;

    private bool timeEnding = false;

    public void NewGame()
    {
        dayOfWeek = 1;
        day = 1;

        dayRemainingTime = dayTime;
        TXT_Day.text = day.ToString();
        SetWeekDay();
    }

    public void SetDay(int n)
    {
        day = n;
        TXT_Day.text = day.ToString();
        SetWeekDay();
    }

    private void Update()
    {
        if (!GameManager._instance.paused)
        {
            if (dayRemainingTime > 0)
            {
                UpdateTime();

                if(!timeEnding && dayRemainingTime < dayTime / 4)
                {
                    timeEnding = true;
                    GameManager._instance.fxManager.PlaySound("time_ending");
                }

            }
            else
            {
                NewDay();
            }
        }
    }

    private void NewDay()
    {
        dayOfWeek++;
        timeEnding = false;
        /*
        if (dayOfWeek > weekDays)
        {
            GameManager._instance.Pause();
            GameManager._instance.notificationManager.ShowNotification("end_week");
        }
        else
        {
            day++;
            if(day > 30)
            {
                day = 1;
            }

            dayRemainingTime = dayTime;
            TXT_Day.text = day.ToString();

            HandleColor();
            SetWeekDay();

            GameManager._instance.NextDay();
        }*/
        day++;
        if (day > 30)
        {
            day = 1;
        }

        dayRemainingTime = dayTime;
        TXT_Day.text = day.ToString();

        HandleColor();
        SetWeekDay();

        GameManager._instance.NextDay();
    }

    public void NewWeek()
    {
        dayOfWeek = 0;
        NewDay();
        GameManager._instance.Resume();
    }

    private void HandleColor()
    {
        Color save = radialBar.color;

        radialBar.color = radialLayout.color;
        radialLayout.color = save;
    }

    private void SetWeekDay()
    {
        switch (dayOfWeek)
        {
            case 1:
                TXT_WeekDay.text = "Lunes";
                break;
            case 2:
                TXT_WeekDay.text = "Martes";
                break;
            case 3:
                TXT_WeekDay.text = "Miércoles";
                break;
            case 4:
                TXT_WeekDay.text = "Jueves";
                break;
            case 5:
                TXT_WeekDay.text = "Viernes";
                break;
            case 6:
                TXT_WeekDay.text = "Sábado";
                break;
            case 7:
                TXT_WeekDay.text = "Domigo";
                break;
        }

    }

    private void UpdateTime()
    {
        dayRemainingTime -= Time.deltaTime;
        UpdateTimeBar();
    }

    private void UpdateTimeBar()
    {
        radialBar.fillAmount = (dayRemainingTime / dayTime);
    }

    public void RestTime(int time)
    {
        dayRemainingTime -= time;
        UpdateTimeBar();
    }

}
