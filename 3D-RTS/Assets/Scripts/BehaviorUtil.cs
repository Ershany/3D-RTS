using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorUtil
{
    // util class that holds behavior functions it would probably setup the destination of groups 

    //gonna need some max force and max speed here 
    public void Flock(List<Group> group)
    {
        //Some hardcoded values for speed and etc...
        float maxForce = 0.5f;
        float maxSpeed = 0.5f;
        float neighborDistance = 100.0f;
        float separationDistance = 2.0f;

        //hardcoded separation atm almost a separation array alignment array and cohesion array
        List<Vector3> separation = Separation(group , separationDistance , maxForce , maxSpeed);
        List<Vector3> alignment = Alignment(group , neighborDistance , maxForce , maxSpeed);
        List<Vector3> cohesion = Cohesion(group , neighborDistance , maxSpeed , maxForce);
        int count = 0;

        //apply all these as forces to build flocking behavior
        for (int i = 0; i < group.Count; i++)
        {
            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                
                Rigidbody rb = group[i].GetUnits()[j].GetRigidbody();
                rb.AddForce(separation[count]);
                rb.AddForce(alignment[count]);
                rb.AddForce(5 * cohesion[count]);
                
                group[i].GetUnits()[j].GetAgent().velocity += separation[count].normalized;
                group[i].GetUnits()[j].GetAgent().velocity += alignment[count];
                group[i].GetUnits()[j].GetAgent().velocity += cohesion[count];
                group[i].GetUnits()[j].GetAgent().velocity = group[i].GetUnits()[j].GetAgent().velocity.normalized * group[i].GetUnits()[j].GetAgent().speed;

                count++;
            }
        }
    }

    //Calculates steering vector for units so that they can separate from each other
    //Separation between units in a flock
    List<Vector3> Separation(List<Group> group , float separation , float maxForce , float maxSpeed)
    {
        List<Vector3> steer = new List<Vector3>();
        int count = 0;

        for (int i = 0; i < group.Count; i++)
        {
            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                steer.Add(new Vector3(0 , 0 , 0));
                
                for (int k = 0; k < group[i].GetUnits().Count; k++)
                {
                    if (j == k) continue;

                    float distance = Vector3.Distance(group[i].GetUnits()[j].GetTransform().position, group[i].GetUnits()[k].GetTransform().position);
                    //Debug.Log(distance);

                    //these 2 units are 2 close steer them away
                    if ( distance < separation)
                    {
                        //find opposite vector from unit to steer in that direction and normalize it
                        Vector3 difference = group[i].GetUnits()[j].GetTransform().position - group[i].GetUnits()[k].GetTransform().position;
                        difference = difference.normalized / distance; //????????? WHY DIVIDE BY DISTANCE THAT DOESN"T MAKE SENSE
                        steer[j] += difference;
                        count++;
                    }
                }

                //average the steering for each unit
                if (count > 0)
                {
                    steer[j] /= count;

                    // if we have a steering position
                    if (steer[j].magnitude != 0)
                    {
                        //Reynolds steering : steering = desired - velocity
                        steer[j] = steer[j].normalized * maxSpeed;
                        //subtraction of velocity needs to occur at indvidual units
                        steer[j] -= group[i].GetUnits()[j].GetAgent().velocity;
                        //limit the force by the max we can allow
                        if (steer[j].magnitude > maxForce)
                        {
                            steer[j] = steer[j].normalized * maxForce;
                        }
                    }
                }

                //reset counter
                count = 0;
            }
        }

        return steer;
    }

    //Alignment of units in a flock
    List<Vector3> Alignment(List<Group> group , float neighborDistance , float maxForce , float maxSpeed)
    {
        List<Vector3> steer = new List<Vector3>();
        int count = 0;

        for (int i = 0; i < group.Count; i++)
        {
            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                steer.Add(new Vector3(0, 0, 0));

                for (int k = 0; k < group[i].GetUnits().Count; k++)
                {
                    if (j == k) continue;

                    float distance = Vector3.Distance(group[i].GetUnits()[j].GetTransform().position, group[i].GetUnits()[k].GetTransform().position);

                    if (distance < neighborDistance)
                    {
                        //add velocities of all closeby neighbors
                        steer[j] += group[i].GetUnits()[k].GetAgent().velocity;
                        count++;
                    }
                }

                //average the steering for each unit (average the velocities)
                if (count > 0)
                {
                    steer[j] /= count;

                    //Reynolds steering : steering = desired - velocity
                    steer[j] = steer[j].normalized * maxSpeed;
                    //subtraction of velocity needs to occur at indvidual units
                    steer[j] -= group[i].GetUnits()[j].GetAgent().velocity;
                    //limit the force by the max we can allow
                    if (steer[j].magnitude > maxForce)
                    {
                        steer[j] = steer[j].normalized * maxForce;
                    }
                }
                else
                {
                    steer[j] = new Vector3(0, 0, 0);
                }
                //reset counter
                count = 0;
            }
        }

        return steer;
    }

    //Cohesion between units of a flock
    List<Vector3> Cohesion(List<Group> group, float neighborDistance , float maxSpeed , float maxForce)
    {
        List<Vector3> steer = new List<Vector3>();
        int count = 0;

        for (int i = 0; i < group.Count; i++)
        {
            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                steer.Add(new Vector3(0, 0, 0));

                for (int k = 0; k < group[i].GetUnits().Count; k++)
                {
                    if (j == k) continue;

                    float distance = Vector3.Distance(group[i].GetUnits()[j].GetTransform().position, group[i].GetUnits()[k].GetTransform().position);
                    
                    //these 2 units are neighbors
                    if (distance < neighborDistance)
                    {
                        steer[j] += group[i].GetUnits()[k].GetGameObject().transform.position;
                        count++;
                    }
                }

                if (count > 0)
                {
                    steer[j] /= count;
                    steer[j] = Seek(group[i].GetUnits()[j] , steer[j] , maxSpeed , maxForce);
                }
                else
                {
                    steer[j] = new Vector3(0, 0, 0);
                }
            }
        }

        return steer;
    }





    //Seek function
    public Vector3 Seek(DynamicUnit unit , Vector3 seekPosition , float maxSpeed , float maxForce)
    {
        //get direction vector to the target position
        Vector3 desired = -unit.GetGameObject().transform.position + seekPosition;
        desired = desired.normalized * maxSpeed;

        //Reynolds steering
        Vector3 steeringVector = desired - unit.GetAgent().velocity;

        //setup threshold for the maxForce you can apply
        if (steeringVector.magnitude > maxForce)
        {
            steeringVector = steeringVector.normalized * maxForce;
        }

        //return the vector needed to steer to 
        return steeringVector;
    }

    // implement a wander function according to notes
    public void Wander(List<Group> group)
    {
        float angle = 5.0f;
        float radius = 5.0f;
        float displacement = 30.0f; // no idea

        for (int i = 0; i < group.Count; i++)
        {
            //set up movement for entire group to keep cohesion between the units of the group
            //get a random theta from the movement range
            float theta = Random.Range(-angle, angle);

            Debug.Log(theta);
            //find target position + the radius
            Vector3 target = new Vector3(radius * Mathf.Cos(theta) , 0.0f , radius * Mathf.Sin(theta));

            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                //Quaternion rotation = Quaternion.AngleAxis(theta, new Vector3(1 , 0 , 1));
                //Debug.Log("my rotation: " + rotation);

                //Vector3 newForward = rotation * group[i].GetUnits()[0].GetAgent().velocity;

                //Debug.Log("new velocity: " + newForward);
                //group[i].GetUnits()[j].GetTransform().Rotate(newForward);

                //group[i].GetUnits()[j].GetAgent().velocity = newForward.normalized * 5.0f;

                //hardcoded speed 
                group[i].GetUnits()[j].GetAgent().velocity =  (target + group[i].GetUnits()[j].GetAgent().velocity).normalized * 500.0f * Time.deltaTime;

                //Rotate the unit to align to this new movement
                //Vector3 center = group[i].GetUnits()[j].GetAgent().velocity.normalized * displacement;
                //target += center;

                //what is player_center ????
                //target += group[i].GetUnits()[j].GetGameObject().transform.position;

                //threshold movement
                //target = target.normalized * 3.0f;

                //set it to be the velocity
                //group[i].GetUnits()[j].GetRigidbody().AddForce(target);
                //group[i].GetUnits()[j].GetAgent().velocity = target;
            }
        }
    }

    //lerp function with ease in and ease out 
    //delta time calculation should be done before given into the function 
    public  Vector3 Lerp(Vector3 start , Vector3 end , float t)
    {
        //ease t parameter for ease in and ease out lerping
        float easedT = Ease(t);

        //regular linear interpolation
        return (1 - easedT) * start + easedT * end;
    }

    //Ease function that determines whether we are in ease in or ease out 
    public float Ease(float t)
    {
        return (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2.0f) + 1.0f) / 2.0f;
    }

    //Ease in linear interpolation
    static float EaseIn(float t)
    {
        // t^2
        return t * t;
    }

    //Ease out linear interpolation
    static float EaseOut(float t)
    {
        // 1 - (1 - t) ^ 2
        return 1 - ((1 - t) * (1 - t));
    }

    public void Patrol()
    {
    }

    public void Attack()
    {
    }

    public void Run()
    {
    }

    public void Evade()
    {
    }

    public void ExploreConservative()
    {
    }

    public void ExploreBravely()
    {
    }

    public void FindLoot()
    {
    }
}
