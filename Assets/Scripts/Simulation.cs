using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public PathManager pathManager;
    private List<Agent> agents = new List<Agent>();

    void Start()
    {
        // Find all agents in the scene and ensure they are registered with the PathManager
        foreach (var agentObject in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Agent agent = agentObject.GetComponent<Agent>();
            if (agent != null)
            {
                agents.Add(agent);
                if (pathManager == null)
                {
                    pathManager = FindObjectOfType<PathManager>();
                }

                if (pathManager != null)
                {
                    pathManager.RegisterAgent(agent); // Ensure each agent is registered
                }
                else
                {
                    Debug.LogError("PathManager is not assigned to Simulator.");
                }
            }
        }
    }

    void Update()
    {
        // This could be used for simulation-wide updates if necessary in the future
    }
}
