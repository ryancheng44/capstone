using System;
using UnityEngine;
using TMPro;

public class DateManager : MonoBehaviour
{
    [Header("Initial Values")]
    [SerializeField] private int startYear = 2024;
    [SerializeField] private int startMonth = 1;
    [SerializeField] private int startDay = 1;

    [Header("Miscellaneous")]
    [SerializeField] private float timeBetweenDays = 1f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dateText;

    public static DateManager instance { get; private set; }
    public event Action OnNewMonth;
    public DateTime date { get; private set; }

    private float timeSinceLastDay = 0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        date = new DateTime(startYear, startMonth, startDay);
    }

    // Start is called before the first frame update
    void Start() => AddDays(0);

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDay += Time.deltaTime;

        if (timeSinceLastDay >= timeBetweenDays)
        {
            AddDays(1);
            timeSinceLastDay = 0;
        }
    }

    private void AddDays(int days)
    {
        DateTime nextDay = date.AddDays(days);

        foreach (House house in PopulationManager.instance.AllHouses())
            if (nextDay >= house.nextTaxDate)
                house.MakeTaxable();

        if (nextDay.Month != date.Month)
            OnNewMonth?.Invoke();

        date = nextDay;
        dateText.text = $"{date.ToString("MMMM")} {GetDayWithSuffix(date.Day)}, {date.Year}";
    }

    private string GetDayWithSuffix(int day)
    {
        if (day >= 11 && day <= 13)
            return $"{day}th";

        switch (day % 10)
        {
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
