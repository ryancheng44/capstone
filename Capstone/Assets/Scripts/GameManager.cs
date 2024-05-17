// CLEARED

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LevelComplete()
    {
        if (gameOver)
            return;

        levelCompletePanel.SetActive(true);
    }

    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void Continue()
    {
        LevelManager.Instance.NextLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Menu() => SceneManager.LoadScene("Menu Scene");
}
