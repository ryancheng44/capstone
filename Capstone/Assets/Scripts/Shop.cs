using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Shop : MonoBehaviour
{
    [SerializeField] private Tower[] towerPrefabs;
    [SerializeField] private TowerButton towerButtonPrefab;
    [SerializeField] private Transform content;

    private Animator animator;
    private bool isHidden = false;

    // Start is called before the first frame update
    void Start()
    {
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
}
