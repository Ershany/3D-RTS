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
            Bird obj = Instantiate(birdPrefab, Bird.RandomWanderPos(), Quaternion.identity, this.gameObject.transform).GetComponent<Bird>();
            obj.BeginWander(Bird.RandomWanderPos());
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
