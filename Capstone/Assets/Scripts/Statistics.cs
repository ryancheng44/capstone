using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    public static Statistics instance { get; private set; }

    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI balanceText;

    [SerializeField] private float startingBalance = 1000;

    public float xMin { get; private set; }
    public float XMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    private int population = 0;
    private float balance = 0;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        xMin = leftBorder.position.x;
        XMax = rightBorder.position.x;
        yMin = bottomBorder.position.y;
        yMax = topBorder.position.y;

        SetBalance(startingBalance);
    }

    public int GetPopulation() => population;

    public void SetPopulation(int value) {
        population = value;
        populationText.text = "Population: " + population;
    }

    public float GetBalance() => balance;

    public void SetBalance(float value) {
        balance = value;
        balanceText.text = "Balance: " + balance.ToString("C2");
    }
}
