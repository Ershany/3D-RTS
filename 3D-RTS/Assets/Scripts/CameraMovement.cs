using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform cameraTarget;
    public float smoothing = 0.5f;
    public float cameraSpeed = 50.0f;
    public float mouseDeadzoneOffset = 45.0f;

    private Vector3 cameraOffset;

	void Awake ()
    {
        cameraOffset = new Vector3(0.0f, transform.position.y, -20.0f); // Used for camera targeting
    }

    void Update()
    {
        // Free movement
        if (cameraTarget == null)
        {
            float horizontalMovement = Mathf.Clamp(Input.GetAxis("Horizontal") + CheckHorizontalMouse(), -1.0f, 1.0f);
            float verticalMovement = Mathf.Clamp(Input.GetAxis("Vertical") + CheckVerticalMouse(), -1.0f, 1.0f);

            Vector3 destination = new Vector3(transform.position.x + horizontalMovement * cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z + verticalMovement * cameraSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, destination, cameraSpeed * Time.deltaTime);
        }
    }

    void LateUpdate ()
    {
        // Targeting movement
        if (cameraTarget)
        {
            Vector3 destination = cameraTarget.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, destination, smoothing * Time.deltaTime);
        }
    }

    float CheckHorizontalMouse()
    {
        if (Input.mousePosition.x < mouseDeadzoneOffset) return -1.0f;
        if (Input.mousePosition.x > Screen.width - mouseDeadzoneOffset) return 1.0f;
        return 0.0f;
    }

    float CheckVerticalMouse()
    {
        if (Input.mousePosition.y < mouseDeadzoneOffset) return -1.0f;
        if (Input.mousePosition.y > Screen.height - mouseDeadzoneOffset) return 1.0f;
        return 0.0f;
    }
}
