using UnityEngine;
using TMPro;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] private House housePrefab;

    [Header("Initial Values")]
    [SerializeField] private int startMinNumHouses = 8;
    [SerializeField] private int startMaxNumHouses = 12;
    [field: SerializeField] public int minNumAdults { get; private set; } = 1;
    [field: SerializeField] public int maxNumAdults { get; private set; } = 2;
    public float chanceToHaveChildren { get; private set; } = 0f;

    [Header("Misceallaneous")]
    [SerializeField] private int maxAttemptsToSpawnHouse = 100;
    [SerializeField] private float minDistanceBetweenHouses = 1f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI populationText;

    public int population { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < Random.Range(startMinNumHouses, startMaxNumHouses + 1); i++)
            SpawnHouse();
    }

    private void SpawnHouse() {
        Vector2 randomPosition = Vector2.zero;

        for (int i = 0; i < maxAttemptsToSpawnHouse; i++)
        {
            randomPosition.x = Random.Range(Boundaries.instance.xMin, Boundaries.instance.xMax);
            randomPosition.y = Random.Range(Boundaries.instance.yMin, Boundaries.instance.yMax);

            if (Physics2D.OverlapCircle(randomPosition, minDistanceBetweenHouses) == null)
            {
                Instantiate(housePrefab, randomPosition, Quaternion.identity);
                return;
            }
        }

        print("Couldn't spawn house after " + maxAttemptsToSpawnHouse + " attempts.");
    }
    
    public void SetPopulation(int value) {
        population = value;
        populationText.text = "Population: " + population;
    }
}
