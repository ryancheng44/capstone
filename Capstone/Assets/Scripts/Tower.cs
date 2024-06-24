// Potentially allow cost to be influenced by Events and other things

using UnityEngine;

public class Tower : MonoBehaviour
{
    [field: SerializeField] public Sprite ProfilePic { get; private set; }
    [field: SerializeField] public string TowerName { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }

    public float CurrentRange { get; private set; }

    [SerializeField] private LayerMask germsLayer;
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private float attackRate;
    [SerializeField] private float damage;
    [SerializeField] private float range;

    private float currentAttackRate;
    private float currentDamage;

    public int SellValue => totalValue / 2;
    private int totalValue = 0;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;
        SR.sortingOrder = 0;

        totalValue = Cost;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f)
            TryAttack();
        else
            timer -= Time.deltaTime;
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    public void OnEffectsChange()
    {
        currentAttackRate = attackRate * (1.0f + EffectsManager.Instance.Effects["Tower Attack Rate"]);
        currentDamage = damage * (1.0f + EffectsManager.Instance.Effects["Tower Damage"]);
        CurrentRange = range * (1.0f + EffectsManager.Instance.Effects["Tower Range"]);
    }

    private void TryAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, CurrentRange, germsLayer);

        if (colliders.Length == 0)
            return;

        Attack(colliders);
        timer = 1f / currentAttackRate;
    }

    private void Attack(Collider2D[] colliders)
    {
        Transform furthestGerm = null;
        int furthestWaypointIndex = -1;

        foreach (Collider2D collider in colliders)
        {
            int currentWaypointIndex = collider.GetComponent<Germ>().CurrentWaypointIndex;

            if (currentWaypointIndex > furthestWaypointIndex)
            {
                furthestGerm = collider.transform;
                furthestWaypointIndex = currentWaypointIndex;
            }
        }

        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Init(furthestGerm, currentDamage);
    }
}
