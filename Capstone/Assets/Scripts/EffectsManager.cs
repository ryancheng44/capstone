// CLEARED

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }
    [HideInInspector] public UnityEvent onEffectsChange;
    public Dictionary<string, float> Effects { get; private set; } = new();

    private Dictionary<string, float> eventEffects = new();
    private Dictionary<string, float> antibodyEffects = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Effects = new()
        {
            { "Germ Antibodies Awarded", 0.0f },
            { "Germ Damage Per Second", 0.0f },
            { "Germ Health", 0.0f },
            { "Germ Spawn Rate", 0.0f },
            { "Germ Speed", 0.0f },
            { "Tower Attack Rate", 0.0f },
            { "Tower Damage", 0.0f },
            { "Tower Range", 0.0f }
        };

        onEffectsChange.Invoke();
    }

    public void OnAntibodiesChange(float effect)
    {
        RemoveEffects(antibodyEffects);

        antibodyEffects = new()
        {
            { "Germ Antibodies Awarded", effect },
            { "Germ Damage Per Second", -effect },
            { "Germ Health", -effect },
            { "Germ Spawn Rate", -effect },
            { "Germ Speed", effect },
            { "Tower Attack Rate", effect },
            { "Tower Damage", effect },
            { "Tower Range", effect }
        };

        ApplyEffects(antibodyEffects);
        onEffectsChange.Invoke();
    }

    public void OnEventConclusion(Dictionary<string, float> effectsDict)
    {
        eventEffects = effectsDict;
        ApplyEffects(eventEffects);
    }

    public void OnNewEvent() => RemoveEffects(eventEffects);

    private void ApplyEffects(Dictionary<string, float> effectsDict)
    {
        foreach (string key in effectsDict.Keys)
            Effects[key] += effectsDict[key];

        onEffectsChange.Invoke();
    }

    private void RemoveEffects(Dictionary<string, float> effectsDict)
    {
        foreach (string key in effectsDict.Keys)
            Effects[key] -= effectsDict[key];

        onEffectsChange.Invoke();
    }
}
