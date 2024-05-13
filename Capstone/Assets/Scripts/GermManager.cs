using System.Collections.Generic;
using UnityEngine;

public class GermManager : MonoBehaviour
{
    public static GermManager Instance { get; private set; }

    private Level currentLevel;
    private int currentWaveIndex = 0;
    private Wave currentWave;

    private Germ currentGerm;
    private int germsToSpawn = 0;
    private int germsAlive = 0;
    private float spawnRate;
    private float currentSpawnRate;

    private float timer = 0.0f;

    private Dictionary<string, float> effectsDict = new ();

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
        if (currentWaveIndex >= currentLevel.waves.Length)
            return;

        currentWave = currentLevel.waves[currentWaveIndex];

        currentGerm = currentWave.germ;
        germsToSpawn = currentWave.count;
        spawnRate = currentWave.spawnRate;

        OnEffectsChange(effectsDict);
        currentWaveIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f && germsToSpawn > 0)
        {
            Germ germ = Instantiate(currentGerm, transform.position, Quaternion.identity);
            germ.OnEffectsChange(effectsDict);

            germsAlive++;
            germsToSpawn--;

            if (germsToSpawn <= 0)
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

    private void OnEffectsChange(Dictionary<string, float> effectsDict)
    {
        currentSpawnRate = spawnRate * (1.0f + effectsDict["Germ Spawn Rate"]);
        this.effectsDict = effectsDict;
    }

    public void GermDied()
    {
        germsAlive--;

        if (germsAlive <= 0 && currentWaveIndex >= currentLevel.waves.Length)
            GameManager.Instance.LevelComplete();
    }
}
