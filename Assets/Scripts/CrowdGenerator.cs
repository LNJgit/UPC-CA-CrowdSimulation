using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private int N = 20;
    [SerializeField] private float MinimumDistance = 2.0f;
    [SerializeField] private float spawnRange = 20.0f;

    private List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        int spawnedObjects = 0;

        while (spawnedObjects < N)
        {
            Vector3 spawnPosition = GenerateRandomPosition();

            bool success = false;
            for (int attempt = 0; attempt < 100; attempt++)
            {
                if (IsValidPosition(spawnPosition))
                {
                    positions.Add(spawnPosition);

                    GameObject agentObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

                    spawnedObjects++;
                    success = true;
                    break;
                }
                else
                {
                    spawnPosition = GenerateRandomPosition();
                }
            }

            if (!success)
            {
                Debug.LogWarning($"Failed to place object {spawnedObjects + 1} after 100 attempts.");
                break;
            }
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float x = Random.Range(-spawnRange / 2, spawnRange / 2);
        float z = Random.Range(-spawnRange / 2, spawnRange / 2);
        return new Vector3(x, 1, z);
    }

    private bool IsValidPosition(Vector3 spawnPosition)
    {
        foreach (var position in positions)
        {
            if (Vector3.Distance(position, spawnPosition) < MinimumDistance)
            {
                return false;
            }
        }
        return true;
    }
}
