// Potentially allow cost to be influenced by Events and other things

using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Unity Stuff")]
    [SerializeField] protected LayerMask germsLayer;
    [field: SerializeField] public Sprite ProfilePic { get; private set; }

    [Header("Stats")]
    [SerializeField] private float attackRate;
    [field: SerializeField] public int Cost { get; private set; }
    [SerializeField] private float damage;
    [SerializeField] protected float range;

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

    protected virtual void Attack() => timer = 1f / currentAttackRate;

    public void OnEffectsChange(Dictionary<string, float> effectsDict)
    {
        currentAttackRate = attackRate * (1.0f + effectsDict["Tower Attack Rate"]);
        currentDamage = damage * (1.0f + effectsDict["Tower Damage"]);
        currentRange = range * (1.0f + effectsDict["Tower Range"]);
    }
}
