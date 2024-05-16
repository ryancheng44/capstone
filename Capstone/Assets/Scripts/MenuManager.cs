using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject guidePanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject content;
    [SerializeField] private LevelButton levelButtonPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CreateLevelButton(int levelIndex) => Instantiate(levelButtonPrefab, content.transform).Init(levelIndex);

    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void LevelSelect()
    {
        startPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void SelectLevel(int levelIndex)
    {
        LevelManager.Instance.SelectLevel(levelIndex);
        StartGame();
    }

    public void Guide()
    {
        startPanel.SetActive(false);
        guidePanel.SetActive(true);
    }

    public void Back()
    {
        startPanel.SetActive(true);
        guidePanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }
}
