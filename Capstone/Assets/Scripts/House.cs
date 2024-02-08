using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class House : MonoBehaviour
{
    private int numAdults = 0;
    private int numChildren = 0;
    
    void Start()
    {
        numAdults = Random.Range(PopulationManager.instance.minNumAdults, PopulationManager.instance.maxNumAdults + 1);
        PopulationManager.instance.SetPopulation(PopulationManager.instance.population + numAdults);
        TryToHaveChildren();
    }

    private void OnMouseDown() => Debug.Log("House clicked!");
    
    private void OnEnable() => DateManager.instance.OnNewMonth += TryToHaveChildren;
    private void OnDisable() => DateManager.instance.OnNewMonth -= TryToHaveChildren;

    private void TryToHaveChildren() {
        if (numAdults >= 2 && Random.value <= PopulationManager.instance.chanceToHaveChildren) {
            numChildren++;
            PopulationManager.instance.SetPopulation(PopulationManager.instance.population + 1);
        }
    }
}
