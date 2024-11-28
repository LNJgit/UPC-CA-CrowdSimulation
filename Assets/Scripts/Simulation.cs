using System.Collections;
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
}
