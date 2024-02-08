using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 10f;

    private float widthMin = Screen.width * 0.1f;
    private float widthMax = Screen.width * 0.9f;
    private float heightMin = Screen.height * 0.1f;
    private float heightMax = Screen.height * 0.9f;

    // Update is called once per frame
    void Update() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 cameraPosition = transform.position;

        if (mousePosition.x < widthMin)
            cameraPosition.x -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.x > widthMax)
            cameraPosition.x += cameraSpeed * Time.deltaTime;

        if (mousePosition.y < heightMin)
            cameraPosition.y -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.y > heightMax)
            cameraPosition.y += cameraSpeed * Time.deltaTime;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, Boundaries.instance.xMin, Boundaries.instance.xMax);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, Boundaries.instance.yMin, Boundaries.instance.yMax);

        transform.position = cameraPosition;
    }
}
