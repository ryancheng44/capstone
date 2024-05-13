using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Image profilePic;
    [SerializeField] private TextMeshProUGUI costText;
    
    private Tower towerPrefab;

    public void Init(Tower towerPrefab)
    {
        this.towerPrefab = towerPrefab;
        profilePic.sprite = towerPrefab.ProfilePic;
        costText.text = towerPrefab.Cost.ToString();
    }

    public void OnClick() => Shop.instance.SpawnTower(towerPrefab);
}
