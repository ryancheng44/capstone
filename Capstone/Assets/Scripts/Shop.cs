using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Shop : MonoBehaviour
{
    public static Shop instance { get; private set; }

    [SerializeField] private Tower[] towerPrefabs;
    [SerializeField] private TowerButton towerButtonPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject towerPlacementPanel;
    [SerializeField] private TMP_InputField xInputField;
    [SerializeField] private TMP_InputField yInputField;

    private Animator animator;
    private bool isHidden = false;

    private Tower towerToPlace;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
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
    }

    public void Toggle()
    {
        isHidden = !isHidden;
        animator.Play(isHidden ? "Hide" : "Show");
    }

    public void BeginTowerPlacement(Tower towerPrefab)
    {
        towerToPlace = towerPrefab;
        towerPlacementPanel.SetActive(true);
    }

    public void UpdateTowerPosition()
    {
        if (towerToPlace == null)
            return;

        float x = xInputField.text == string.Empty || xInputField.text == "-" ? 0.0f : float.Parse(xInputField.text);
        float y = yInputField.text == string.Empty || yInputField.text == "-" ? 0.0f : float.Parse(yInputField.text);

        towerToPlace.transform.position = new Vector3(x, y, 0.0f);
    }

    public void PlaceTower()
    {
        towerToPlace.isPlaced = true;
        towerToPlace = null;

        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
        towerPlacementPanel.SetActive(false);
    }
}
