using UnityEngine;

public class Tower : MonoBehaviour
{
    [field: SerializeField] public Sprite towerImage { get; private set; }
    [field: SerializeField] public int cost { get; private set; } = 10;

    [SerializeField] protected LayerMask germsLayer;
    [SerializeField] protected float radius = 3.0f;
    [SerializeField] private float cooldown = 3.0f;
    [SerializeField] private float damagePerSecond = 10.0f;
    
    private float currentDamagePerSecond;
    public bool isPlaced = false;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start() => currentDamagePerSecond = damagePerSecond;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
            return;

        if (timer <= 0.0f)
            Attack();
        else
            timer -= Time.deltaTime;
    }

    private void OnEnable()
    {
        EventManager.instance.onEventConclusion.AddListener(OnEventConclusion);
        EventManager.instance.onNewEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        EventManager.instance.onEventConclusion.RemoveListener(OnEventConclusion);
        EventManager.instance.onNewEvent.RemoveListener(Reset);
    }

    protected virtual void Attack() => timer = cooldown;

    private void OnEventConclusion(Event e, bool correct) => damagePerSecond = currentDamagePerSecond * (1.0f + e.effectOnTowerDamage * (correct ? 1.0f : -1.0f));
    private void Reset() => currentDamagePerSecond = damagePerSecond;
}
