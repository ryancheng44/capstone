using UnityEngine;

public enum FourBasicOperations
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

[CreateAssetMenu(fileName = "New Custom Event", menuName = "Events/Custom Event")]
public class Event : ScriptableObject
{
    public new string name;

    [Range(1, 3)]
    public int firstNumberDigits;
    [Range(1, 3)]
    public int secondNumberDigits;
    
    public FourBasicOperations[] operationsAllowed;
    public bool negativesAllowed;
    public float time;

    public float effectOnGermSpeed = 0.0f;
    public float effectOnGermSpawnRate = 0.0f;
    public float effectOnGermHealth = 0.0f;
    public float effectOnGermDamage = 0.0f;
    public float effectOnTowerDamage = 0.0f;
}

[CreateAssetMenu(fileName = "New Type I Event", menuName = "Events/Type I Event")]
public class TypeIEvent : Event
{
    public TypeIEvent()
    {
        firstNumberDigits = 1;
        secondNumberDigits = 1;
        operationsAllowed = new FourBasicOperations[]
        {
            FourBasicOperations.Addition,
            FourBasicOperations.Subtraction
        };
        negativesAllowed = false;
        time = 3.0f;
    }
}

[CreateAssetMenu(fileName = "New Type II Event", menuName = "Events/Type II Event")]
public class TypeIIEvent : Event
{
    public TypeIIEvent()
    {
        firstNumberDigits = 2;
        secondNumberDigits = 1;
        operationsAllowed = new FourBasicOperations[]
        {
            FourBasicOperations.Addition,
            FourBasicOperations.Subtraction
        };
        negativesAllowed = false;
        time = 3.0f;
    }
}

[CreateAssetMenu(fileName = "New Type III Event", menuName = "Events/Type III Event")]
public class TypeIIIEvent : Event
{
    public TypeIIIEvent()
    {
        firstNumberDigits = 2;
        secondNumberDigits = 2;
        operationsAllowed = new FourBasicOperations[]
        {
            FourBasicOperations.Addition,
            FourBasicOperations.Subtraction
        };
        negativesAllowed = false;
        time = 5.0f;
    }
}

[CreateAssetMenu(fileName = "New Type IV Event", menuName = "Events/Type IV Event")]
public class TypeIVEvent : Event
{
    public TypeIVEvent()
    {
        firstNumberDigits = 2;
        secondNumberDigits = 2;
        operationsAllowed = new FourBasicOperations[]
        {
            FourBasicOperations.Addition,
            FourBasicOperations.Subtraction,
            FourBasicOperations.Multiplication
        };
        negativesAllowed = false;
        time = 5.0f;
    }
}
