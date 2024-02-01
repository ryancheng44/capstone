using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float cameraSpeed = 5f;

    private float widthMin, widthMax, heightMin, heightMax;

    // Start is called before the first frame update
    void Start()
    {
        widthMin = Screen.width * 0.1f;
        widthMax = Screen.width * 0.9f;
        heightMin = Screen.height * 0.1f;
        heightMax = Screen.height * 0.9f;
    }

    // Update is called once per frame
    void Update() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 cameraPosition = mainCamera.position;

        if (mousePosition.x < widthMin)
            cameraPosition.x -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.x > widthMax)
            cameraPosition.x += cameraSpeed * Time.deltaTime;

        if (mousePosition.y < heightMin)
            cameraPosition.y -= cameraSpeed * Time.deltaTime;
        else if (mousePosition.y > heightMax)
            cameraPosition.y += cameraSpeed * Time.deltaTime;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, Statistics.instance.xMin, Statistics.instance.XMax);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, Statistics.instance.yMin, Statistics.instance.yMax);

        mainCamera.position = cameraPosition;
    }
}
