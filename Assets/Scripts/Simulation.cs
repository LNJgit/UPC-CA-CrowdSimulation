using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public PathManager pathManager;
    private List<Agent> agents = new List<Agent>();

    void Start()
    {
       
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
                    pathManager.RegisterAgent(agent); 
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
        
    }
}
