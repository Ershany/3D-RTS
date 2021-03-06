﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTarget;
    [Range(0.1f, 10.0f)] public float smoothing = 2.0f;
    [Range(0.0f, 1000.0f)] public float cameraSpeed = 50.0f;
    [Range(0.0f, 300.0f)] public float mouseDeadzoneOffset = 1.0f;
    [Range(10.0f, 60.0f)] public float minCameraFOV = 40.0f;
    [Range(500.0f, 2000.0f)] public float cameraZoomSpeed = 1000.0f;

    private PlayerController player;
    private float cameraFOV, maxCameraFOV;
    private Vector3 cameraOffset;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        maxCameraFOV = cameraFOV = Camera.main.fieldOfView;
        cameraOffset = new Vector3(0.0f, transform.position.y, -35.0f); // Used for camera targeting
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //check if there is a selected group
            if (player.selectedGroups.Count > 0)
            {
                cameraTarget = cameraTarget == null ? player.selectedGroups[0].GetFirstUnit().GetTransform() : null;
            }
        }

        // Free Movement
        if (cameraTarget == null)
        {
            float horizontalMovement = Mathf.Clamp(Input.GetAxis("Horizontal") + CheckHorizontalMouse(), -1.0f, 1.0f);
            float verticalMovement = Mathf.Clamp(Input.GetAxis("Vertical") + CheckVerticalMouse(), -1.0f, 1.0f);

            Vector3 destination = new Vector3(transform.position.x + horizontalMovement * cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z + verticalMovement * cameraSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, destination, cameraSpeed * Time.deltaTime);
        }

        ProcessMouseScroll();
    }

    void LateUpdate()
    {
        // Targeting movement
        if (cameraTarget)
        {
            Vector3 destination = cameraTarget.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, destination, smoothing * Time.deltaTime);
        }
    }

    void ProcessMouseScroll()
    {
        // If UI is selected, don't process mouse scroll because the UI should be processing the mouse scrolling
        if (player.buildingSelected == null)
        {
            float deltaScroll = Input.GetAxis("Mouse ScrollWheel"); // Zoom out = negative      Zoom in = positive
            cameraFOV += -(deltaScroll * cameraZoomSpeed * Time.deltaTime);
            if (cameraFOV > maxCameraFOV) cameraFOV = maxCameraFOV;
            else if (cameraFOV < minCameraFOV) cameraFOV = minCameraFOV;
            if (Camera.main.orthographic)
            {
                Camera.main.orthographicSize = cameraFOV;
            }
            else
                Camera.main.fieldOfView = cameraFOV;
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
