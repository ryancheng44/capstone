// CLEARED

using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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

    [SerializeField] private Transform towerCanvas;
    [SerializeField] private float towerCanvasOffset;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerSellValueText;
    [SerializeField] private TextMeshProUGUI towerUpgradeCostText;

    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private LayerMask towerLayer;

    private Tower towerToPlace = null;
    private Tower selectedTower = null;

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

        if (Input.GetMouseButtonUp(0) && towerToPlace == null && !EventSystem.current.IsPointerOverGameObject())
        {
            Tower previousTower = selectedTower;
            DeselectTower();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, towerLayer);

            if (hit.collider != null)
            {
                Tower tower = hit.collider.GetComponent<Tower>();

                if (tower != previousTower)
                {
                    selectedTower = tower;
                    selectedTower.GetComponent<SpriteRenderer>().color = Color.green;

                    ShowOptions();
                    ShowRangeIndicator(selectedTower);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (towerToPlace != null)
            {
                Destroy(towerToPlace.gameObject);
                towerToPlace = null;
                ClearTowerPlacementStuff();
            }
            else
                DeselectTower();
        }
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    private void OnEffectsChange()
    {
        if (towerToPlace != null)
            ShowRangeIndicator(towerToPlace);
        else if (selectedTower != null)
            ShowRangeIndicator(selectedTower);
    }

    public void Toggle()
    {
        isHidden = !isHidden;
        animator.SetBool("isHidden", isHidden);
    }

    private void ShowRangeIndicator(Tower tower)
    {
        // Understandably questionable
        // Methods subscribed to a UnityEvent are called in the order they were added
        // Therefore we have to intentionally call OnEffectsChange() before updating the range indicator
        // Otherwise the range indicator will be updated before the tower's range is updated
        // (Since towers are born after Shop, so they are added to the UnityEvent after Shop is added)
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

    private void ShowOptions()
    {
        towerCanvas.position = selectedTower.transform.position + Vector3.up * towerCanvasOffset;

        towerNameText.text = selectedTower.TowerName;
        towerSellValueText.text = selectedTower.SellValue.ToString();

        towerCanvas.gameObject.SetActive(true);
    }

    private void HideOptions() => towerCanvas.gameObject.SetActive(false);

    public void SpawnTower(Tower towerPrefab)
    {
        if (towerToPlace != null)
        {
            Destroy(towerToPlace.gameObject);
            towerToPlace = null;
        }

        DeselectTower();

        towerToPlace = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        towerPlacementPanel.SetActive(true);

        UpdateTowerPosition();
        ShowRangeIndicator(towerToPlace);
    }

    public void UpdateTowerPosition()
    {
        if (towerToPlace == null)
            return;

        float x = float.TryParse(xInputField.text, out float parsedX) ? parsedX : 0;
        float y = float.TryParse(yInputField.text, out float parsedY) ? parsedY : 0;

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

        towerToPlace = null;
        ClearTowerPlacementStuff();
    }

    private void ClearTowerPlacementStuff()
    {
        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
        towerPlacementPanel.SetActive(false);

        HideRangeIndicator();
    }

    private void DeselectTower()
    {
        if (selectedTower == null)
            return;

        selectedTower.GetComponent<SpriteRenderer>().color = Color.white;
        selectedTower = null;

        HideOptions();
        HideRangeIndicator();
    }

    public void SellTower()
    {
        AntibodyManager.Instance.ChangeAntibodiesBy(selectedTower.SellValue);

        HideOptions();
        HideRangeIndicator();

        Destroy(selectedTower.gameObject, 0.1f);
        selectedTower = null;
    }
}
