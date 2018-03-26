using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    //would need an animation to run first 

    //hack way of doing things
    float timer;
    bool isActive;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(false);
        timer = 0;
        isActive = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            //add amount of time elapsed
            timer += Time.deltaTime;

            //check for deactivating the marker
            if (timer > 1.5f)
            {
                isActive = false;
                gameObject.SetActive(false);
                timer = 0;
            }
        }
    }

    public void ActivateMarker(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        timer = 0;
        isActive = true;
    }
}
