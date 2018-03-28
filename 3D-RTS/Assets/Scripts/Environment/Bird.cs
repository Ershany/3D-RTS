using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public float speed;
    public List<Bird> flock;

    enum BirdState
    {
        SEEK, FLOCK, WANDER
    }

    // Bird variables
    private Rigidbody rb;
    private BirdState currentState;
    private BehaviorUtil util;
    private Vector3 velocity;

    // Seek variables
    public Vector3 seekPosition;

    // Wander variables
    private float currentWanderAngle;
    private float currentT;
    private Vector3 wanderPos;
    private Vector3 startPos;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentState = BirdState.SEEK;
        util = new BehaviorUtil();
        velocity = Vector3.zero;

        seekPosition = new Vector3(100.0f, 25.0f, 0.0f);

        currentWanderAngle = 0.0f;
        wanderPos = RandomWanderPos();
        startPos = this.transform.position;
        currentT = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Seek
        if (currentState == BirdState.SEEK)
        {
            // Updated velocity
            Vector3 desiredVelocity = seekPosition - this.transform.position;
            desiredVelocity = desiredVelocity.normalized * speed * Time.deltaTime;

            Vector3 steering = desiredVelocity - velocity;

            velocity += steering;

            // Update unit position
            transform.position += velocity;

            this.transform.LookAt(seekPosition);
        }

        // Wander
        else if (currentState == BirdState.WANDER)
        {
            currentT += Time.deltaTime * (1.0f / (wanderPos - startPos).magnitude) * speed;
            this.transform.position = util.Lerp(startPos, wanderPos, currentT);
            this.transform.LookAt(wanderPos);

            // Select new random spot to wander
            if (currentT >= 1.0f)
            {
                // x [0, 512], y 25, z [0, 512]
                BeginWander(RandomWanderPos());
            }
        }
    }

    public void Flock()
    {
        float seperationDist = 10.0f;
        float neighborDist = 15.0f;
        Vector3 steer = Vector3.zero;
        int count = 0;

        // Seperation
        foreach (Bird bird in flock)
        {
            if (bird == this) continue;

            Vector3 diff = this.transform.position - bird.transform.position;
            float distance = diff.magnitude;
            if (distance < seperationDist)
            {
                diff = diff.normalized / distance;
                steer += -diff;
                ++count;
            }
        }

        if (count > 0)
        {
            steer /= count;
            //change here
            //steer = steer.normalized * speed;
            velocity += steer.normalized;
        }

        //reset steer
        steer = Vector3.zero;

        //alignment 
        foreach (Bird bird in flock)
        {
            if (bird == this) continue;

            Vector3 diff = this.transform.position - bird.transform.position;
            float distance = diff.magnitude;

            if (distance < neighborDist)
            {

                steer += bird.velocity;
                ++count;
            }
        }

        if (count > 0)
        {
            //average velocities
            steer /= count;
            steer = steer.normalized * speed;
            steer -= velocity;

            velocity += steer;
        }


        //reset steer
        steer = Vector3.zero;


        //cohesion
        foreach (Bird bird in flock)
        {
            if (bird == this) continue;

            Vector3 diff = this.transform.position - bird.transform.position;
            float distance = diff.magnitude;

            if (distance < neighborDist)
            {
                steer += bird.transform.position;
                ++count;
            }
        }

        if (count > 0)
        {
            steer /= count;

            //seek that new average position
            Vector3 desiredVelocity = steer - this.transform.position;
            //desiredVelocity = desiredVelocity.normalized * speed * Time.deltaTime;

            //i think
            Vector3 steering = desiredVelocity.normalized - velocity.normalized;

            velocity += steering.normalized;

            // Update unit position
            // transform.position += velocity;

            //this.transform.LookAt(desiredVelocity);
        }

        //setup velocity vector
        velocity = velocity.normalized * speed;
    }

    public static Vector3 RandomWanderPos()
    {
        return new Vector3(Random.Range(0.0f, 512.0f), 25.0f, Random.Range(0.0f, 512.0f));
    }

    public void BeginWander(Vector3 positionToWander)
    {
        currentState = BirdState.WANDER;
        startPos = this.transform.position;
        wanderPos = positionToWander;
        currentT = 0.0f;
    }
}
