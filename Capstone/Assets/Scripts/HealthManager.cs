// Potentially allow maxHealth and healthRegen to be affected by Events and other things

using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthRegenText;

    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRegen;

    private float currentHealth;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
        healthRegenText.text = healthRegen.ToString();
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
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        healthText.text = Mathf.CeilToInt(currentHealth).ToString();
    }
}
