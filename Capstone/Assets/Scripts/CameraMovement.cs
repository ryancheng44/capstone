using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    [SerializeField] private float cameraSpeed = 5f;

    private float xMin, xMax, yMin, yMax;
    private float widthMin, widthMax, heightMin, heightMax;

    // Start is called before the first frame update
    void Start()
    {
        xMin = leftBorder.position.x;
        xMax = rightBorder.position.x;
        yMin = bottomBorder.position.y;
        yMax = topBorder.position.y;

        widthMin = Screen.width * 0.1f;
        widthMax = Screen.width * 0.9f;
        heightMin = Screen.height * 0.1f;
        heightMax = Screen.height * 0.9f;
    }

    // Update is called once per frame
    void Update() => MoveCamera();

    private void MoveCamera()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 cameraPosition = transform.position;

        if (mousePosition.x < widthMin)
            cameraPosition.x -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.x > widthMax)
            cameraPosition.x += cameraSpeed * Time.deltaTime;

        if (mousePosition.y < heightMin)
            cameraPosition.y -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.y > heightMax)
            cameraPosition.y += cameraSpeed * Time.deltaTime;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, xMin, xMax);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, yMin, yMax);

        transform.position = cameraPosition;
    }
}
