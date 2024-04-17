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
        currentSpeed = speed;
        currentHealth = health;
        currentDamagePerSecond = damagePerSecond;

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

    private void OnEnable() => AntibodyManager.instance.onAntibodiesChange.AddListener(Buff);
    private void OnDisable() => AntibodyManager.instance.onAntibodiesChange.RemoveListener(Buff);

    private void Buff(float buff)
    {
        currentSpeed = speed * (1.0f - buff);
        currentHealth = health * (1.0f + buff);
        currentDamagePerSecond = damagePerSecond * (1.0f + buff);
    }
}
