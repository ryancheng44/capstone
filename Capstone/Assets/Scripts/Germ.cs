using System.Collections.Generic;
using UnityEngine;

public class Germ : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int antibodiesAwarded;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float health;
    [SerializeField] private float speed;

    private int currentAntibodiesAwarded;
    private float currentDamagePerSecond;
    private float currentHealth;
    private float currentSpeed;
    private float totalDamageTaken = 0.0f;

    public int currentWaypointIndex { get; private set; } = 0;
    private Vector3 currentWaypoint;
    private float threshold = 0.1f;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = Path.Points[currentWaypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            HealthManager.instance.TakeDamage(currentDamagePerSecond);
            timer = 0.0f;
        }

        if (Vector3.Distance(transform.position, currentWaypoint) < threshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= Path.Points.Length)
            {
                GermManager.Instance.GermDied();
                Destroy(gameObject);
                return;
            }
            currentWaypoint = Path.Points[currentWaypointIndex];
        }

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, currentSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        totalDamageTaken += damage;

        if (totalDamageTaken >= currentHealth)
        {
            AntibodyManager.Instance.ChangeAntibodiesBy(currentAntibodiesAwarded);
            GermManager.Instance.GermDied();
            Destroy(gameObject);
        }
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    public void OnEffectsChange(Dictionary<string, float> effectsDict)
    {
        currentAntibodiesAwarded = (int)(antibodiesAwarded * (1.0f + effectsDict["Germ Antibodies Awarded"]));
        currentDamagePerSecond = damagePerSecond * (1.0f + effectsDict["Germ Damage Per Second"]);
        currentHealth = health * (1.0f + effectsDict["Germ Health"]);
        currentSpeed = speed * (1.0f + effectsDict["Germ Speed"]);
    }
}
