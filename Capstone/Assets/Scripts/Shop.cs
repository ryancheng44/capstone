// CLEARED

using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }

    [SerializeField] private Tower[] towerPrefabs;
    [SerializeField] private TowerButton towerButtonPrefab;
    [SerializeField] private Transform content;

    [SerializeField] private GameObject towerPlacementPanel;
    [SerializeField] private TMP_InputField xInputField;
    [SerializeField] private TMP_InputField yInputField;
    [SerializeField] private LayerMask offLimitsLayer;

    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private Transform towerCanvas;
    [SerializeField] private float towerCanvasOffset;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerSellValueText;
    [SerializeField] private TextMeshProUGUI towerUpgradeCostText;
    [SerializeField] private LayerMask towerLayer;

    private Tower selectedTower = null;
    private Tower towerToPlace = null;

    private Animator animator;
    private bool isHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (Tower towerPrefab in towerPrefabs)
            Instantiate(towerButtonPrefab, content).Init(towerPrefab);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Toggle();

        if (Input.GetMouseButtonUp(0) && towerToPlace == null)
        {
            DeselectTower();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, towerLayer);

            if (hit.collider != null)
            {
                selectedTower = hit.collider.GetComponent<Tower>();
                selectedTower.GetComponent<SpriteRenderer>().color = Color.green;
                ShowOptions();
                ShowRangeIndicator(selectedTower);
            }
            
            // TODO: Make clicking on UI not deselect the tower
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (towerToPlace != null)
            {
                Destroy(towerToPlace.gameObject);
                ClearTowerPlacementStuff();
            }
            else
                DeselectTower();
        }
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    private void DeselectTower()
    {
        if (selectedTower == null)
            return;

        selectedTower.GetComponent<SpriteRenderer>().color = Color.white;
        selectedTower = null;
        HideRangeIndicator();
        HideOptions();
    }

    private void OnEffectsChange()
    {
        if (towerToPlace != null || selectedTower != null)
            ShowRangeIndicator(towerToPlace != null ? towerToPlace : selectedTower);
    }

    public void ShowOptions()
    {
        towerCanvas.SetParent(selectedTower.transform);
        towerCanvas.localPosition = new Vector3(0f, towerCanvasOffset, 0f);

        towerNameText.text = selectedTower.TowerName;
        towerSellValueText.text = selectedTower.SellValue.ToString();
        if (selectedTower.HasBeenUpgraded)
            towerUpgradeCostText.text = "MAX";
        else
            towerUpgradeCostText.text = selectedTower.UpgradeCost.ToString();

        towerCanvas.gameObject.SetActive(true);
    }

    public void HideOptions()
    {
        towerCanvas.SetParent(null);
        towerCanvas.gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {
        if (selectedTower.HasBeenUpgraded)
            return;

        selectedTower.Upgrade();
    }

    public void SellTower()
    {
        HideOptions();
        HideRangeIndicator();
        selectedTower.Sell();
        DeselectTower();
    }

    public void Toggle()
    {
        isHidden = !isHidden;
        animator.SetBool("isHidden", isHidden);
    }

    public void SpawnTower(Tower towerPrefab)
    {
        if (towerToPlace != null)
        {
            Destroy(towerToPlace.gameObject);
            ClearTowerPlacementStuff();
        }

        towerToPlace = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        towerPlacementPanel.SetActive(true);

        ShowRangeIndicator(towerToPlace);
    }

    public void UpdateTowerPosition()
    {
        if (towerToPlace == null)
            return;

        if (float.TryParse(xInputField.text, out float x)) { }
        else x = 0;

        if (float.TryParse(yInputField.text, out float y)) { }
        else y = 0;

        towerToPlace.GetComponent<Rigidbody2D>().MovePosition(new Vector2(x, y));
    }

    public void PlaceTower()
    {
        if (towerToPlace.GetComponent<Collider2D>().IsTouchingLayers(offLimitsLayer))
        {
            Debug.Log("Can't place tower here");

            // TODO: Show error message

            return;
        }

        AntibodyManager.Instance.ChangeAntibodiesBy(-towerToPlace.Cost);

        towerToPlace.enabled = true;
        ClearTowerPlacementStuff();
    }

    private void ClearTowerPlacementStuff()
    {
        towerToPlace = null;

        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
        towerPlacementPanel.SetActive(false);

        HideRangeIndicator();
    }

    private void ShowRangeIndicator(Tower tower)
    {
        tower.OnEffectsChange();

        Vector3 temp = rangeIndicator.localScale;
        temp.x = tower.CurrentRange;
        rangeIndicator.localScale = temp;

        rangeIndicator.SetParent(tower.transform);
        rangeIndicator.localPosition = Vector3.zero;

        rangeIndicator.gameObject.SetActive(true);
    }

    private void HideRangeIndicator()
    {
        rangeIndicator.SetParent(null);
        rangeIndicator.gameObject.SetActive(false);
    }
}
