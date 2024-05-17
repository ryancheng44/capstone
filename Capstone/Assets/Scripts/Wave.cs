// CLEARED

using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public Germ germ;
    public int count;
    public float spawnRate;
}
