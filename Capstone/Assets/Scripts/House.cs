using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class House : MonoBehaviour
{
    private int numAdults = 0;
    private int numChildren = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        DateManager.instance.OnNewMonth += PopulationAttraction;
        DateManager.instance.OnNewMonth += ChanceToHaveChildren;

        numAdults = Random.Range(PopulationManager.instance.minNumAdults, PopulationManager.instance.maxNumAdults + 1);
        PopulationManager.instance.SetPopulation(PopulationManager.instance.population + numAdults);
        ChanceToHaveChildren();
    }

    private void OnMouseDown() => Debug.Log($"This house has {numAdults} adults and {numChildren} children.");

    private void ChanceToHaveChildren() {
        if (numAdults >= 2 && Random.value <= PopulationManager.instance.chanceToHaveChildren) {
            numChildren++;
            PopulationManager.instance.SetPopulation(PopulationManager.instance.population + 1);
        }
    }

    private void PopulationAttraction() {
        if (numAdults < PopulationManager.instance.maxNumAdults && Random.value <= PopulationManager.instance.populationAttraction) {
            numAdults++;
            PopulationManager.instance.SetPopulation(PopulationManager.instance.population + 1);

            if (numAdults >= PopulationManager.instance.maxNumAdults)
                DateManager.instance.OnNewMonth -= PopulationAttraction;
        }
    }
}
