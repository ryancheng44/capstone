using UnityEngine;

public class GermManager : MonoBehaviour
{
    [SerializeField] private Germ[] germPrefabs;

    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 2.0f;

    private float currentMinSpawnInterval;
    private float currentMaxSpawnInterval;

    private Event currentEvent;
    private bool correct;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentMinSpawnInterval = minSpawnInterval;
        currentMaxSpawnInterval = maxSpawnInterval;

        timer = Random.Range(currentMinSpawnInterval, currentMaxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f)
        {
            Germ germ = Instantiate(germPrefabs[Random.Range(0, germPrefabs.Length)], transform.position, Quaternion.identity);
            
            if (currentEvent == null)
                germ.Reset();
            else
                germ.OnEventConclusion(currentEvent, correct);

            timer = Random.Range(currentMinSpawnInterval, currentMaxSpawnInterval);
        }
        else
            timer -= Time.deltaTime;
    }
    
    private void OnEnable()
    {
        EventManager.instance.onEventConclusion.AddListener(OnEventConclusion);
        EventManager.instance.onNewEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        EventManager.instance.onEventConclusion.RemoveListener(OnEventConclusion);
        EventManager.instance.onNewEvent.RemoveListener(Reset);
    }

    private void OnEventConclusion(Event e, bool correct)
    {
        currentMinSpawnInterval = minSpawnInterval * (1.0f + e.effectOnGermSpawnRate * (correct ? 1.0f : -1.0f));
        currentMaxSpawnInterval = maxSpawnInterval * (1.0f + e.effectOnGermSpawnRate * (correct ? 1.0f : -1.0f));

        currentEvent = e;
        this.correct = correct;
    }

    private void Reset()
    {
        currentMinSpawnInterval = minSpawnInterval;
        currentMaxSpawnInterval = maxSpawnInterval;

        currentEvent = null;
        correct = false;
    }
}
