// CLEARED

using UnityEngine;
using UnityEngine.UI;

public class Germ : MonoBehaviour
{
    [SerializeField] private Scrollbar healthBar;
    [SerializeField] private int antibodiesAwarded;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float health;
    [SerializeField] private float speed;

    private int currentAntibodiesAwarded;
    private float currentDamagePerSecond;
    private float currentHealth;
    private float currentSpeed;

    public int CurrentWaypointIndex { get; private set; } = 1;
    private Vector3 currentWaypoint;

    private float totalDamageTaken = 0.0f;
    private bool isDead = false;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = Path.Points[CurrentWaypointIndex];
        OnEffectsChange();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            HealthManager.Instance.TakeDamage(currentDamagePerSecond);
            timer = 0.0f;
        }

        Vector3 direction = currentWaypoint - transform.position;
        float distanceThisFrame = currentSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            transform.position = currentWaypoint;

            CurrentWaypointIndex++;
            if (CurrentWaypointIndex >= Path.Points.Length)
            {
                GermManager.Instance.GermDied();
                Destroy(gameObject);
                return;
            }
            currentWaypoint = Path.Points[CurrentWaypointIndex];
        }
        else
            transform.position += direction.normalized * distanceThisFrame;
    }

    public void TakeDamage(float damage)
    {
        totalDamageTaken += damage;

        if (totalDamageTaken >= currentHealth && !isDead)
        {
            isDead = true;
            AntibodyManager.Instance.ChangeAntibodiesBy(currentAntibodiesAwarded);
            GermManager.Instance.GermDied();
            Destroy(gameObject);
            return;
        }

        healthBar.size = 1.0f - (totalDamageTaken / currentHealth);
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    private void OnEffectsChange()
    {
        currentAntibodiesAwarded = (int)(antibodiesAwarded * (1.0f + EffectsManager.Instance.Effects["Germ Antibodies Awarded"]));
        currentDamagePerSecond = damagePerSecond * (1.0f + EffectsManager.Instance.Effects["Germ Damage Per Second"]);
        currentHealth = health * (1.0f + EffectsManager.Instance.Effects["Germ Health"]);
        currentSpeed = speed * (1.0f + EffectsManager.Instance.Effects["Germ Speed"]);

        TakeDamage(0.0f);
    }
}
