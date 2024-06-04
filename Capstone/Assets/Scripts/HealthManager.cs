// Potentially allow healthRegen to be affected by Events and other things
// CLEARED

using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthRegenText;

    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRegen;

    private float currentHealth;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        healthRegenText.text = "+" + healthRegen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            currentHealth = Mathf.Min(currentHealth + healthRegen, maxHealth);
            healthText.text = Mathf.CeilToInt(currentHealth).ToString();

            timer = 0.0f;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            GameManager.Instance.GameOver();
            currentHealth = 0.0f;
        }

        healthText.text = Mathf.CeilToInt(currentHealth).ToString();
    }
}
