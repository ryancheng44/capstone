using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject housePrefab;

    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    private float xMin, xMax, yMin, yMax;

    [SerializeField] private int minNumberOfHouses = 8;
    [SerializeField] private int maxNumberOfHouses = 12;

    // Start is called before the first frame update
    void Start()
    {
        xMin = leftBorder.position.x;
        xMax = rightBorder.position.x;
        yMin = bottomBorder.position.y;
        yMax = topBorder.position.y;

        for (int i = 0; i < Random.Range(minNumberOfHouses, maxNumberOfHouses + 1); i++)
            Instantiate(housePrefab, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
