using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public static Boundaries instance { get; private set; }

    [Header("Boundaries")]
    [SerializeField] private Transform topBoundary;
    [SerializeField] private Transform bottomBoundary;
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;

    public float xMin { get; private set; }
    public float xMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        xMin = leftBoundary.position.x;
        xMax = rightBoundary.position.x;
        yMin = bottomBoundary.position.y;
        yMax = topBoundary.position.y;
    }
}
