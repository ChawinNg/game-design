using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public Transform TargetTransform;

    private float smoothAmount = 0.2f;

    private Transform cameraTransform;

    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = GetComponent<Camera>().GetComponent<Transform>();
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        Vector2 newPosition = Vector2.Lerp(lastCameraPosition, TargetTransform.position, smoothAmount);
        cameraTransform.position = new Vector3(newPosition.x, newPosition.y, lastCameraPosition.z);

        lastCameraPosition = cameraTransform.position;
    }
}
