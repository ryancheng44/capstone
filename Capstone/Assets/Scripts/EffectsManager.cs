using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }
    [HideInInspector] public UnityEvent<Dictionary<string, float>> onEffectsChange;
    private Dictionary<string, float> effectsDict;

    private Dictionary<string, float> previousEffectsDict = new ();
    private float previousEffect = 0.0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        effectsDict = new ()
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
    }

    // Start is called before the first frame update
    void Start() => onEffectsChange.Invoke(effectsDict);

    public void OnAntibodiesChange(float effect)
    {
        RemovePreviousEffect();

        effectsDict["Germ Antibodies Awarded"] += effect;
        effectsDict["Germ Damage Per Second"] -= effect;
        effectsDict["Germ Health"] -= effect;
        effectsDict["Germ Spawn Rate"] -= effect;
        effectsDict["Germ Speed"] += effect;
        effectsDict["Tower Attack Rate"] += effect;
        effectsDict["Tower Damage"] += effect;
        effectsDict["Tower Range"] += effect;

        onEffectsChange.Invoke(effectsDict);
        previousEffect = effect;
    }

    private void RemovePreviousEffect()
    {
        effectsDict["Germ Antibodies Awarded"] -= previousEffect;
        effectsDict["Germ Damage Per Second"] += previousEffect;
        effectsDict["Germ Health"] += previousEffect;
        effectsDict["Germ Spawn Rate"] += previousEffect;
        effectsDict["Germ Speed"] -= previousEffect;
        effectsDict["Tower Attack Rate"] -= previousEffect;
        effectsDict["Tower Damage"] -= previousEffect;
        effectsDict["Tower Range"] -= previousEffect;
    }

    public void OnEventConclusion(Dictionary<string, float> effectsDict)
    {
        foreach (var key in effectsDict.Keys)
            this.effectsDict[key] += effectsDict[key];

        onEffectsChange.Invoke(this.effectsDict);
        previousEffectsDict = effectsDict;
    }

    public void OnNewEvent()
    {
        if (previousEffectsDict.Count == 0)
            return;

        foreach (var key in new List<string>(effectsDict.Keys))
            effectsDict[key] -= previousEffectsDict[key];
        
        onEffectsChange.Invoke(effectsDict);
    }
}
