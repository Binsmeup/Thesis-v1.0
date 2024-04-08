using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2; // Speed of camera dragging
    public float zoomSpeed = 2; // Speed of zooming

    private Vector3 dragOrigin;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Camera dragging
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += difference;
        }

        // Camera zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scroll * zoomSpeed, 1f, 20f);
        }
    }
}