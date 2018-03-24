using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    public GameObject birdPrefab;
    public float birdHeight;

    private List<Bird> birds;
    private Vector3 flockPosition;

	// Use this for initialization
	void Start () {
        birds = new List<Bird>();
        flockPosition = Bird.RandomWanderPos();

        // Wander birds
        for (int i = 0; i < 10; ++i)
        {
            Bird obj = Instantiate(birdPrefab, new Vector3(i * 3, birdHeight, i * 3), Quaternion.identity).GetComponent<Bird>();
            obj.BeginWander(Bird.RandomWanderPos());
        }

        // Flocking Birds
        for (int i = 0; i < 10; ++i)
        {
            Bird obj = Instantiate(birdPrefab, new Vector3(i * 3, birdHeight, 0.0f), Quaternion.identity).GetComponent<Bird>();
            birds.Add(obj);
            obj.flock = birds;
        }
    }
	
	// Update is called once per frame
	void Update () {
		foreach (Bird bird in birds)
        {
            bird.Flock();
            bird.seekPosition = flockPosition;

            if ((bird.transform.position - flockPosition).magnitude < 0.25f)
            {
                flockPosition = Bird.RandomWanderPos();
            }
        }


	}
}
