// CLEARED

using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    public string successDescription;
    public string failureDescription;

    public bool affectGermAntibodiesAwarded;
    public bool affectGermDamagePerSecond;
    public bool affectGermHealth;
    public bool affectGermSpawnRate;
    public bool affectGermSpeed;
    public bool affectTowerAttackRate;
    public bool affectTowerDamage;
    public bool affectTowerRange;
}
