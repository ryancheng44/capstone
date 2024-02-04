using System;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    public static Statistics instance { get; private set; }

    [Header("Initial Values")]
    [SerializeField] private int startYear = 2024;
    [SerializeField] private int startMonth = 1;
    [SerializeField] private int startDay = 1;
    [SerializeField] private float startBalance = 1000;

    private DateTime date;
    private float balance = 0;
    private int population = 0;
    
    [Header("Misc")]
    [SerializeField] private float timeBetweenDays = 5f;
    private float timeSinceLastDay = 0;

    [Header("Borders")]
    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    public float xMin { get; private set; }
    public float XMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI populationText;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        SetDate(new DateTime(startYear, startMonth, startDay));
        SetBalance(startBalance);

        xMin = leftBorder.position.x;
        XMax = rightBorder.position.x;
        yMin = bottomBorder.position.y;
        yMax = topBorder.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDay += Time.deltaTime;

        if (timeSinceLastDay >= timeBetweenDays) {
            timeSinceLastDay = 0;
            date = date.AddDays(1);
            SetDate(date);
        }
    }

    public DateTime GetDate() => date;

    public void SetDate(DateTime value) {
        date = value;
        string dayWithSuffix = GetDayWithSuffix(date.Day);
        dateText.text = $"{date.ToString("MMMM")} {dayWithSuffix}, {date.Year}";
    }

    public float GetBalance() => balance;

    public void SetBalance(float value) {
        balance = value;
        balanceText.text = "Balance: " + balance.ToString("C2");
    }

    public int GetPopulation() => population;

    public void SetPopulation(int value) {
        population = value;
        populationText.text = "Population: " + population;
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
