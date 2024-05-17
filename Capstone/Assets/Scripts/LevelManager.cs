// CLEARED

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private Level[] levels;

    private int currentLevelIndex = 0;
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

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].operationsAllowed = new HashSet<FourBasicOperations>(levels[i].operationsAllowed).ToArray();
            MenuManager.Instance.CreateLevelButton(i);
        }

        currentLevel = levels[currentLevelIndex];
    }

    public void NextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex > levels.Length)
        {
            Debug.Log("No more levels");
            return;
        }

        currentLevel = levels[currentLevelIndex];
    }

    public void SelectLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex];
    }
}
