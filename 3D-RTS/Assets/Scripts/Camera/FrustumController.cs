using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumController : MonoBehaviour {

    private List<GameObject> playerObjectsInView;

    void Awake()
    {
        playerObjectsInView = new List<GameObject>();    
    }

    public void AddObjectInView(GameObject gameObj)
    {
        playerObjectsInView.Add(gameObj);
    }

    public bool RemoveObjectInView(GameObject gameObj)
    {
        return playerObjectsInView.Remove(gameObj);
    }
}
