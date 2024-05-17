// CLEARED

using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    private int levelIndex;

    public void Init(int levelIndex)
    {
        this.levelIndex = levelIndex;
        levelText.text = (levelIndex + 1).ToString();
    }

    public void OnClick() => MenuManager.Instance.SelectLevel(levelIndex);
}
