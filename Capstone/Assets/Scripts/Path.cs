using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    public static Vector3[] Points { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(Points);
    }
}
