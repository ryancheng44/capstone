using UnityEngine;

public class Germ : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float health = 10.0f;
    [SerializeField] private float damagePerSecond = 1.0f;
    [SerializeField] private int antibodiesAwarded = 10;

    private float currentSpeed;
    private float currentHealth;
    private float currentDamagePerSecond;

    [HideInInspector] public int currentWaypointIndex;
    [HideInInspector] public Vector3 currentWaypoint;
    private float threshold = 0.1f;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypointIndex = 0;
        currentWaypoint = Path.instance.points[currentWaypointIndex];
        transform.position = currentWaypoint;
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
            if (currentWaypointIndex >= Path.instance.points.Length)
            {
                if (currentWaypointIndex == int.MaxValue)
                    AntibodyManager.instance.ChangeAntibodiesBy(antibodiesAwarded);
                else
                    Debug.Log("Germ reached the end of the path");

                Destroy(gameObject);
                return;
            }
            currentWaypoint = Path.instance.points[currentWaypointIndex];
        }

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, currentSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        AntibodyManager.instance.onAntibodiesChange.AddListener(OnAntibodiesChange);
        EventManager.instance.onEventConclusion.AddListener(OnEventConclusion);
        EventManager.instance.onNewEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        AntibodyManager.instance.onAntibodiesChange.RemoveListener(OnAntibodiesChange);
        EventManager.instance.onEventConclusion.RemoveListener(OnEventConclusion);
        EventManager.instance.onNewEvent.RemoveListener(Reset);
    }

    private void OnAntibodiesChange(float effect)
    {
        currentSpeed = speed * (1.0f - effect);
        currentHealth = health * (1.0f + effect);
        currentDamagePerSecond = damagePerSecond * (1.0f + effect);
    }

    public void OnEventConclusion(Event e, bool correct)
    {
        currentSpeed = speed * (1.0f + e.effectOnGermSpeed * (correct ? 1.0f : -1.0f));
        currentHealth = health * (1.0f + e.effectOnGermHealth * (correct ? 1.0f : -1.0f));
        currentDamagePerSecond = damagePerSecond * (1.0f + e.effectOnGermDamage * (correct ? 1.0f : -1.0f));
    }

    public void Reset()
    {
        currentSpeed = speed;
        currentHealth = health;
        currentDamagePerSecond = damagePerSecond;
    }
}
