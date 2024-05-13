using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [SerializeField] private Level[] levels;
    private int currentLevelIndex = 1;
    public Level currentLevel { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Level level in levels)
            level.operationsAllowed = new HashSet<FourBasicOperations>(level.operationsAllowed).ToArray();
        
        currentLevel = levels[currentLevelIndex - 1];
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        
        if (currentLevelIndex > levels.Length)
        {
            Debug.Log("No more levels");
            return;
        }

        currentLevel = levels[currentLevelIndex - 1];
    }

    public void SelectLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex - 1];
    }
}
