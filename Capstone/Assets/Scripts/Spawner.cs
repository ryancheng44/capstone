using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private int minNumberOfHouses = 8;
    [SerializeField] private int maxNumberOfHouses = 12;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(minNumberOfHouses, maxNumberOfHouses + 1); i++) {
            float randomX = Random.Range(Statistics.instance.xMin, Statistics.instance.XMax);
            float randomY = Random.Range(Statistics.instance.yMin, Statistics.instance.yMax);
            
            Instantiate(housePrefab, new Vector2(randomX, randomY), Quaternion.identity);
            Statistics.instance.AddPopulation();
        }
    }
}
