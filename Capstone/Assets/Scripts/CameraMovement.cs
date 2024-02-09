using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 10f;

    // Update is called once per frame
    void Update() {
        Vector3 cameraPosition = transform.position;

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        cameraPosition.x += horizontalMovement * cameraSpeed * Time.deltaTime;
        cameraPosition.y += verticalMovement * cameraSpeed * Time.deltaTime;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, Boundaries.instance.xMin, Boundaries.instance.xMax);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, Boundaries.instance.yMin, Boundaries.instance.yMax);

        transform.position = cameraPosition;
    }
}
