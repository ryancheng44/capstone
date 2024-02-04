using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject housePrefab;

    [Header("Initial Values")]
    [SerializeField] private int minStartNumberOfHouses = 8;
    [SerializeField] private int maxStartNumberOfHouses = 12;
    [SerializeField] private int startMinNumberOfOccupants = 1;
    [SerializeField] private int startMaxNumberOfOccupants = 2;

    private int minNumberOfOccupants;
    private int maxNumberOfOccupants;

    // Start is called before the first frame update
    void Start()
    {
        minNumberOfOccupants = startMinNumberOfOccupants;
        maxNumberOfOccupants = startMaxNumberOfOccupants;

        for (int i = 0; i < Random.Range(minStartNumberOfHouses, maxStartNumberOfHouses + 1); i++)
            SpawnHouse();
    }

    public void SpawnHouse() {
        Instantiate(housePrefab, new Vector2(Random.Range(Statistics.instance.xMin, Statistics.instance.XMax), Random.Range(Statistics.instance.yMin, Statistics.instance.yMax)), Quaternion.identity);
        Statistics.instance.SetPopulation(Statistics.instance.GetPopulation() + Random.Range(minNumberOfOccupants, maxNumberOfOccupants + 1));
    }

    public int GetMinNumberOfOccupants() => minNumberOfOccupants;
    public void SetMinNumberOfOccupants(int min) => minNumberOfOccupants = min;
    
    public int GetMaxNumberOfOccupants() => maxNumberOfOccupants;
    public void SetMaxNumberOfOccupants(int max) => maxNumberOfOccupants = max;
}
