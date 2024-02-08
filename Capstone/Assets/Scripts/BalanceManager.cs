using UnityEngine;
using TMPro;

public class BalanceManager : MonoBehaviour
{
    [Header("Initial Value")]
    [SerializeField] private float balance = 1000f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI balanceText;

    // Start is called before the first frame update
    void Start() => balanceText.text = "Balance: " + balance.ToString("C2");
}
