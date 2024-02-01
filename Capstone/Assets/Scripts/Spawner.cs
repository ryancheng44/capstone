using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private int minNumberOfHouses = 8;
    [SerializeField] private int maxNumberOfHouses = 12;
    [SerializeField] private int minNumberOfOccupants = 1;
    [SerializeField] private int maxNumberOfOccupants = 4;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(minNumberOfHouses, maxNumberOfHouses + 1); i++) {
            Instantiate(housePrefab, new Vector2(Random.Range(Statistics.instance.xMin, Statistics.instance.XMax), Random.Range(Statistics.instance.yMin, Statistics.instance.yMax)), Quaternion.identity);
            Statistics.instance.SetPopulation(Statistics.instance.GetPopulation() + Random.Range(minNumberOfOccupants, maxNumberOfOccupants + 1));
        }
    }
}
