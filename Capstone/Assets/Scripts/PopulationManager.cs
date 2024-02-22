using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

public class PopulationManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private House housePrefab;

    [Header("Start")]
    [SerializeField] private int minNumHousesAtStart = 8;
    [SerializeField] private int maxNumHousesAtStart = 12;

    [Header("House")]
    [SerializeField] private int maxChildrenPerHouse = 4;

    [Header("Chances")]
    [SerializeField] private float populationAttraction = 0.1f;
    [SerializeField] private float chanceToDie = 0.1f;
    [SerializeField] private float chanceToDate = 0.1f;
    [SerializeField] private float chanceToMarry = 0.5f;
    [SerializeField] private float chanceToHaveChildren = 0.25f;

    [Header("Misceallaneous")]
    [SerializeField] private int maxAttemptsToSpawnHouse = 100;
    [SerializeField] private float minDistanceBetweenHouses = 0.5f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI populationText;

    public static PopulationManager instance { get; private set; }
    private List<House> singleHouses = new List<House>();
    private List<House> datingHouses = new List<House>();
    private List<House> marriedHouses = new List<House>();
    private int population = 0;

    public ReadOnlyCollection<House> AllHouses() => singleHouses.Concat(datingHouses).ToList().Concat(marriedHouses).ToList().AsReadOnly();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < Random.Range(minNumHousesAtStart, maxNumHousesAtStart + 1); i++)
            SpawnHouse();
    }

    // Start is called before the first frame update
    void Start() => DateManager.instance.OnNewMonth += OnNewMonth;

    private void EditPopulationBy(int value)
    {
        population += value;
        populationText.text = "Population: " + population;
    }

    private void SpawnHouse()
    {
        Vector2 randomPosition = Vector2.zero;

        for (int i = 0; i < maxAttemptsToSpawnHouse; i++)
        {
            randomPosition.x = Random.Range(Boundaries.instance.xMin, Boundaries.instance.xMax);
            randomPosition.y = Random.Range(Boundaries.instance.yMin, Boundaries.instance.yMax);

            if (Physics2D.OverlapCircle(randomPosition, minDistanceBetweenHouses) == null)
            {
                House instance = Instantiate(housePrefab, randomPosition, Quaternion.identity);
                instance.numAdults = Random.Range(1, 3);

                if (instance.numAdults == 1)
                    singleHouses.Add(instance);
                else
                    if (ChanceToMarry(instance))
                        if (Random.value <= chanceToHaveChildren)
                            instance.numChildren = Random.Range(1, maxChildrenPerHouse + 1);
                    else
                        datingHouses.Add(instance);

                EditPopulationBy(instance.numAdults + instance.numChildren);

                return;
            }
        }

        print("Couldn't spawn house after " + maxAttemptsToSpawnHouse + " attempts.");
    }

    private void OnNewMonth()
    {
        foreach (House house in AllHouses())
            ChanceToDie(house);

        foreach (House house in marriedHouses)
            if (house.numChildren < maxChildrenPerHouse) ChanceToHaveChildren(house);

        foreach (House house in datingHouses)
            ChanceToMarry(house);

        for (int i = singleHouses.Count - 1; i >= 0; i--)
        {
            if (singleHouses.Count > 1 && Random.value <= chanceToDate)
            {
                House house = singleHouses[i];
                house.numAdults++;
                datingHouses.Add(house);
                singleHouses.Remove(house);

                if (i > 0) i--;

                House houseToBeDestroyed = singleHouses[i];
                singleHouses.Remove(houseToBeDestroyed);
                Destroy(houseToBeDestroyed.gameObject);

                i--;
            }            
        }

        for (int i = 0; i < AllHouses().Count; i++)
            if (Random.value <= populationAttraction)
                SpawnHouse();
    }

    private void ChanceToDie(House house)
    {
        for (int i = 0; i < house.numAdults; i++)
        {
            if (Random.value <= chanceToDie)
            {
                house.numAdults--;
                EditPopulationBy(-1);

                if (house.numAdults < 2 && house.isMarried)
                {
                    house.isMarried = false;
                    marriedHouses.Remove(house);
                    datingHouses.Add(house);
                }

                if (house.numAdults <= 0)
                {
                    House randomMarriedHouse = marriedHouses[Random.Range(0, marriedHouses.Count)];
                    randomMarriedHouse.numChildren += house.numChildren;

                    singleHouses.Remove(house);
                    Destroy(house.gameObject);

                    return;
                }
            }
        }

        for (int i = 0; i < house.numChildren; i++)
        {
            if (Random.value <= chanceToDie * 2)
            {
                house.numChildren--;
                EditPopulationBy(-1);
            }
        }
    }

    private void ChanceToHaveChildren(House house)
    {
        if (Random.value <= chanceToHaveChildren)
        {
            house.numChildren++;
            EditPopulationBy(1);
        }
    }

    private bool ChanceToMarry(House house)
    {
        if (Random.value <= chanceToMarry)
        {
            house.isMarried = true;
            marriedHouses.Add(house);

            return true;
        }

        return false;
    }
}
