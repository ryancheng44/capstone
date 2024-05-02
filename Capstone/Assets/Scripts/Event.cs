using UnityEngine;

public enum FourBasicOperations
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    public string type;
    public string level;

    [Range(1, 3)]
    public int firstNumberDigits;
    [Range(1, 3)]
    public int secondNumberDigits;
    
    public FourBasicOperations[] operationsAllowed;
    public bool negativesAllowed;
    public float time;

    [HideInInspector] public float[] effects;

    public float effectOnGermSpeed = 0.0f;
    public float effectOnGermSpawnRate = 0.0f;
    public float effectOnGermHealth = 0.0f;
    public float effectOnGermDamage = 0.0f;
    public float effectOnTowerDamage = 0.0f;
}
