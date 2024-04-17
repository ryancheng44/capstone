using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] germPrefabs;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 2.0f;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f)
        {
            Instantiate(germPrefabs[Random.Range(0, germPrefabs.Length)], transform.position, Quaternion.identity);
            timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
        else
            timer -= Time.deltaTime;
    }
}
