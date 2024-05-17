// CLEARED

using UnityEngine;

public class GermManager : MonoBehaviour
{
    public static GermManager Instance { get; private set; }

    private Level currentLevel;

    private int currentWaveIndex = 0;
    private Wave currentWave;
    private int germsToSpawn = 0;
    private Germ currentGerm;
    private float spawnRate;
    private float currentSpawnRate;

    private int germsAlive = 0;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentLevel = LevelManager.Instance.currentLevel;
        NewWave();
    }

    private void NewWave()
    {
        currentWave = currentLevel.waves[currentWaveIndex];

        germsToSpawn = currentWave.count;
        currentGerm = currentWave.germ;
        spawnRate = currentWave.spawnRate;

        OnEffectsChange();
        currentWaveIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f && germsToSpawn > 0)
        {
            Instantiate(currentGerm, Path.Points[0], Quaternion.identity);

            germsAlive++;
            germsToSpawn--;

            if (germsToSpawn <= 0 && currentWaveIndex < currentLevel.waves.Length)
            {
                NewWave();
                timer = 1f;
            }
            else
                timer = 1f / currentSpawnRate;
        }
        else
            timer -= Time.deltaTime;
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    private void OnEffectsChange() => currentSpawnRate = spawnRate * (1.0f + EffectsManager.Instance.effectsDict["Germ Spawn Rate"]);

    public void GermDied()
    {
        germsAlive--;

        if (currentWaveIndex >= currentLevel.waves.Length && germsToSpawn <= 0 && germsAlive <= 0)
            GameManager.Instance.LevelComplete();
    }
}
