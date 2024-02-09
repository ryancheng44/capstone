using System;
using UnityEngine;
using TMPro;

public class DateManager : MonoBehaviour
{
    public static DateManager instance { get; private set; }
    public event Action OnNewMonth;

    [Header("Initial Values")]
    [SerializeField] private int startYear = 2024;
    [SerializeField] private int startMonth = 1;
    [SerializeField] private int startDay = 1;
    private DateTime date;

    [Header("Miscellaneous")]
    [SerializeField] private float timeBetweenDays = 1f;
    private float timeSinceLastDay = 0f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dateText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SetDate(new DateTime(startYear, startMonth, startDay));
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDay += Time.deltaTime;

        if (timeSinceLastDay >= timeBetweenDays) {
            timeSinceLastDay = 0;
            DateTime nextDay = date.AddDays(1);

            if (nextDay.Month != date.Month)
                OnNewMonth?.Invoke();
            
            SetDate(nextDay);
        }
    }

    private void SetDate(DateTime value) {
        date = value;
        dateText.text = $"{date.ToString("MMMM")} {GetDayWithSuffix(date.Day)}, {date.Year}";
    }

    private string GetDayWithSuffix(int day) {
        if (day >= 11 && day <= 13)
            return $"{day}th";

        switch (day % 10) {
            case 1:
                return $"{day}st";
            case 2:
                return $"{day}nd";
            case 3:
                return $"{day}rd";
            default:
                return $"{day}th";
        }
    }
}
