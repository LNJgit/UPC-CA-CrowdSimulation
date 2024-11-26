using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public static Simulation Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SimulationCoroutine());
    }

    private System.Collections.IEnumerator SimulationCoroutine()
    {
        while (true)
        {
            PathManager.Instance?.UpdateAgentPaths(); // Delegate to PathManager
            yield return new WaitForSeconds(0.02f);
        }
    }
}
