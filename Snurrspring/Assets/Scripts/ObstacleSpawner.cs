using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private float StartTimeForObstacles = 5.0f;
    private float IntervalForSpawningObstacles = 30.0f;
    public List<Obstacle> ListOfObstacles;

    void Start()
    {
        InvokeRepeating("SpawnObstacles", StartTimeForObstacles, IntervalForSpawningObstacles);
    }

    private IEnumerator InvokeRepeating()
    {
        Debug.Log("Spawning New Obstacles");
        yield return new WaitForEndOfFrame();
    }
    
}
