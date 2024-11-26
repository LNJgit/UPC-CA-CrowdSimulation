using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrowdGenerator : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private int N = 20;
    [SerializeField] private float Minimum_distance = 2.0f;
    System.Random random = new System.Random();
    private List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        Vector3 spawnPosition;
        int maxAttempts = 100;
        int attempts = 0;
        int spawnedObjects = 0;

        while(spawnedObjects<N && attempts < maxAttempts)
        {
            float x  = (float)random.NextDouble()*20-10;
            float z  = (float)random.NextDouble()*20-10;
            spawnPosition  = new Vector3(x,0,z);

            if(IsValidPosition(spawnPosition))
            {
                positions.Add(spawnPosition);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
                spawnedObjects++;
            }
            else
            {
                attempts++;
                if (attempts == 100)
                {
                    Debug.Log("Failed to allocate prefab");
                }
            }
        }
    }

    private bool IsValidPosition(Vector3 spawnPosition)
    {
        foreach (var position in positions)
        {
            if ((position - spawnPosition).magnitude < Minimum_distance)
            {
                return false; // Too close to another object
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
