using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] private LayerMask offLimitsLayer;

    private Animator animator;
    private bool isHidden = false;

    private Tower towerToPlace = null;

    private Dictionary<string, float> effectsDict = new ();

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (towerToPlace != null)
            {
                Destroy(towerToPlace.gameObject);
                towerToPlace = null;
                towerPlacementPanel.SetActive(false);
            }
        }
    }

    private void OnEnable() => EffectsManager.Instance.onEffectsChange.AddListener(OnEffectsChange);
    private void OnDisable() => EffectsManager.Instance.onEffectsChange.RemoveListener(OnEffectsChange);

    private void OnEffectsChange(Dictionary<string, float> effectsDict) => this.effectsDict = effectsDict;

    public void Toggle()
    {
        isHidden = !isHidden;
        animator.Play(isHidden ? "Hide" : "Show");
    }

    public void SpawnTower(Tower towerPrefab)
    {
        if (towerToPlace != null)
            return;
        
        towerToPlace = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        towerPlacementPanel.SetActive(true);
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
            return;
        }
    
        AntibodyManager.Instance.ChangeAntibodiesBy(-towerToPlace.Cost);

        towerToPlace.enabled = true;
        towerToPlace.OnEffectsChange(effectsDict);
        towerToPlace.GetComponent<SpriteRenderer>().color = Color.white;
        towerToPlace = null;

        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
        towerPlacementPanel.SetActive(false);
    }
}
