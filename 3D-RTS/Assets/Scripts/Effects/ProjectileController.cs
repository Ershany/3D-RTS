using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public EffectController explosion;

    private float speed;
    private float timer;
    private bool isActive;
    private Vector3 velocity;
    private Vector3 destination;

    // Use this for initialization
    void Start()
    {
        speed = 40.0f;
        destination = Vector3.zero;
        velocity = Vector3.zero;
        gameObject.SetActive(false);
        isActive = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //add to the timer
        timer += Time.deltaTime;

        // go in the direction of the destination
        if (isActive) { transform.position += velocity * speed * Time.deltaTime; }
        
        //if it has gone for too long just cause an explosion and deactivate it  
        if (timer > 7.0f) { Explode(); }
    }

    //Activate particle system with a position and a destination
    public void Activate(Vector3 position , Vector3 dest)
    {
        //Activate the game object
        gameObject.SetActive(true);
        transform.position = position;
        destination = dest;
        velocity = (destination - transform.position).normalized;

        transform.position += velocity * Time.deltaTime * speed * 8.0f;
        //why do i need to rotate by an extra 90 degrees??????????
        Quaternion rotation = Quaternion.LookRotation(velocity) * Quaternion.AngleAxis(90.0f , new Vector3(0 , -1 , 0));
        transform.rotation = rotation;

        isActive = true;
        timer = 0;
    }

    //create an explosion
    void Explode()
    {
        isActive = false;
        gameObject.SetActive(false);

        //cause explosion at position of spell
        explosion.ActivateMarker(transform.position);
        timer = 0;
    }

    //check for collision detection
    void OnCollisionEnter()
    {
        Explode();
        Debug.Log("hit something");
    }
}
