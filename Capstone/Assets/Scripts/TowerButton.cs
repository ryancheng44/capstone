using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI costText;
    
    private Tower towerPrefab;

    public void Init(Tower towerPrefab)
    {
        this.towerPrefab = towerPrefab;
        towerImage.sprite = towerPrefab.towerImage;
        costText.text = towerPrefab.cost.ToString();
    }

    public void SpawnTower()
    {
        AntibodyManager.instance.ChangeAntibodiesBy(-towerPrefab.cost);
        Instantiate(towerPrefab);
    }
}
