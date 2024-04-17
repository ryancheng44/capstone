using UnityEngine;

public class Tower : MonoBehaviour
{
    [field: SerializeField] public Sprite towerImage { get; private set; }
    [field: SerializeField] public int cost { get; private set; } = 10;

    [SerializeField] protected LayerMask germsLayer;
    [SerializeField] protected float radius = 3.0f;
    [SerializeField] private float cooldown = 3.0f;

    private bool isPlaced = false;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            transform.position = mousePos;

            if (Input.GetMouseButtonDown(0))
                isPlaced = true;
        } else {
            if (timer <= 0.0f)
                Attack();
            else
                timer -= Time.deltaTime;
        }
    }

    protected virtual void Attack() => timer = cooldown;
}
