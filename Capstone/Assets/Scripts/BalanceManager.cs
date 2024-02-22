using UnityEngine;
using TMPro;

public class BalanceManager : MonoBehaviour
{
    [Header("Initial Values")]
    [SerializeField] private float balance = 1000f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI balanceText;

    public static BalanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        EditBalanceBy(0f);
    }

    public void EditBalanceBy(float amount)
    {
        balance += amount;
        balanceText.text = "Balance: " + balance.ToString("C2");
    }
}
