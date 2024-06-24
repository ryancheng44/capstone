// CLEARED

using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask germsLayer;
    [SerializeField] private float speed;
    [SerializeField] private bool splash;
    [SerializeField] private float splashRange;

    private Transform target;
    private float damage;

    public void Init(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.position += transform.up * distanceThisFrame;
    }

    private void HitTarget()
    {
        if (splash)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(target.position, splashRange, germsLayer);

            foreach (Collider2D collider in colliders)
                collider.GetComponent<Germ>().TakeDamage(damage);
        }
        else
            target.GetComponent<Germ>().TakeDamage(damage);

        Destroy(gameObject);
    }
}
