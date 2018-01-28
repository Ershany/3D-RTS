using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float cameraSpeed;
    public float deadZoneOffset;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        Vector3 current = transform.position;
        Vector3 destination = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z + Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(current, destination, cameraSpeed / 10.0f);
    }
}
