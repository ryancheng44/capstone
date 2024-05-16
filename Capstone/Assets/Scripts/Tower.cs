// Potentially allow cost to be influenced by Events and other things

using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Unity Stuff")]
    [SerializeField] private LayerMask germsLayer;
    [field: SerializeField] public Sprite ProfilePic { get; private set; }
    [SerializeField] private Projectile projectilePrefab;

    [Header("Stats")]
    [SerializeField] private float attackRate;
    [field: SerializeField] public int Cost { get; private set; }
    [SerializeField] private float damage;
    [SerializeField] private float range;

    private float currentAttackRate;
    private float currentDamage;
    private float currentRange;

    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f)
            Attack();
        else
            timer -= Time.deltaTime;
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    protected virtual void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentRange, germsLayer);

        if (colliders.Length == 0)
            return;

        Transform furthestGerm = null;
        int furthestWaypointIndex = -1;

        foreach (Collider2D collider in colliders)
        {
            int currentWaypointIndex = collider.GetComponent<Germ>().currentWaypointIndex;
            
            if (currentWaypointIndex > furthestWaypointIndex)
            {
                furthestGerm = collider.transform;
                furthestWaypointIndex = currentWaypointIndex;
            }
        }

        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Init(furthestGerm, currentDamage);

        timer = 1f / currentAttackRate;
    }

    public void OnEffectsChange(Dictionary<string, float> effectsDict)
    {
        currentAttackRate = attackRate * (1.0f + effectsDict["Tower Attack Rate"]);
        currentDamage = damage * (1.0f + effectsDict["Tower Damage"]);
        currentRange = range * (1.0f + effectsDict["Tower Range"]);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }
}
