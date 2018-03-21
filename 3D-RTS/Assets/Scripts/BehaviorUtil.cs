using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorUtil
{
    // util class that holds behavior functions it would probably setup the destination of groups 

    //gonna need some max force and max speed here 
    public static void Flock(List<Group> group)
    {
        //Some hardcoded values for speed and etc...
        float maxForce = 2.0f;
        float maxSpeed = 5.0f;
        float neighborDistance = 100.0f;

        //hardcoded separation atm almost a separation array alignment array and cohesion array
        List<Vector3> separation = Separation(group , neighborDistance , maxForce , maxSpeed);
        List<Vector3> alignment = Alignment(group ,neighborDistance ,maxForce , maxSpeed);
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
                rb.AddForce(cohesion[count]);
                count++;
            }
        }
    }

    //Calculates steering vector for units so that they can separate from each other
    //Separation between units in a flock
    static List<Vector3> Separation(List<Group> group , float separation , float maxForce , float maxSpeed)
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
    static List<Vector3> Alignment(List<Group> group , float neighborDistance , float maxForce , float maxSpeed)
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
    static List<Vector3> Cohesion(List<Group> group, float neighborDistance , float maxSpeed , float maxForce)
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
    public static Vector3 Seek(DynamicUnit unit , Vector3 seekPosition , float maxSpeed , float maxForce)
    {
        // get direction vector to the target position
        Vector3 desired = -unit.GetGameObject().transform.position + seekPosition;
        desired = desired.normalized * maxSpeed;

        //Reynolds steering
        Vector3 steeringVector = desired - unit.GetAgent().velocity;

        if (steeringVector.magnitude > maxForce)
        {
            steeringVector = steeringVector.normalized * maxForce;
        }

        return steeringVector;
    }

    // implement a wander function according to notes
    public static void Wander(List<Group> group)
    {
        //30 degrees angle maybe
        float angle = 30;
        float radius = 5.0f;
        float displacement = 3.0f;

        for (int i = 0; i < group.Count; i++)
        {
            //set up movement for entire group to keep cohesion between the units of the group

            //get a theta from the movement range
            float theta = Random.Range(-angle, angle);

            //move along xz plane (COS ???? SINNNN ????)
            Vector3 target = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));

            for (int j = 0; j < group[i].GetUnits().Count; j++)
            {
                //Rotate the unit to align to this new movement
                Vector3 center = group[i].GetUnits()[j].GetAgent().velocity * displacement;
                target += center;

                //what is player_center
                target += group[i].GetUnits()[j].GetGameObject().transform.position;

                //set it to be the velocity
                group[i].GetUnits()[j].GetAgent().velocity = target;
            }
        }
    }

    //lerp function with ease in and ease out 
    //delta time calculation should be done before given into the function 
    public Vector3 Lerp(Vector3 start , Vector3 end , float t)
    {
        //dunno if this true or not
        float easedT = Ease(t);

        //regular linear interpolation
        return (1 - easedT) * start + easedT * end;
    }

    //Ease function that determines whether we are in ease in or ease out 
    public static float Ease(float t)
    {
        //t is between 0 and 1 (it should be that value)
        if (t < 0.5)
        {
            return EaseIn(t);
        }
        else
        {
            return EaseOut(t);
        }
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

    public void patrol()
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
