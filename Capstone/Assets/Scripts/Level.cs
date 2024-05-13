using UnityEngine;

public enum FourBasicOperations
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public Wave[] waves;
    
    public FourBasicOperations[] operationsAllowed;
    public bool useNegatives;
    public int firstNumberDigits;
    public int secondNumberDigits;
    public float time;

    public float minEffect;
    public float maxEffect;
}
