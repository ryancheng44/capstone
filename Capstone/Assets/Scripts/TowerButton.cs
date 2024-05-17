// CLEARED

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Image profilePicImage;
    [SerializeField] private TextMeshProUGUI costText;
    private Tower towerPrefab;

    public void Init(Tower towerPrefab)
    {
        this.towerPrefab = towerPrefab;
        profilePicImage.sprite = towerPrefab.ProfilePic;
        costText.text = towerPrefab.Cost.ToString();
    }

    public void OnClick() => Shop.Instance.SpawnTower(towerPrefab);
}
