using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance { get; private set; }

    [SerializeField] private float avoidanceRadius = 2.0f; // Personal space radius
    [SerializeField] private float avoidanceWeight = 0.5f; // Weight for avoidance vector
    [SerializeField] private float goalWeight = 0.5f; // Weight for goal direction

    private List<Agent> agents = new List<Agent>();

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

    private void FixedUpdate()
    {
        UpdateAgentPaths(Time.fixedDeltaTime);
    }

    public void RegisterAgent(Agent agent)
    {
        if (!agents.Contains(agent))
        {
            agents.Add(agent);
        }
    }

    public void UpdateAgentPaths(float deltaTime)
    {
        foreach (Agent agent in agents)
        {
            Vector3 goalDirection = (agent.Goal - agent.transform.position).normalized;
            Vector3 avoidanceDirection = CalculateAvoidance(agent);

            // Combine goal direction and avoidance direction
            Vector3 finalDirection = (goalDirection * goalWeight + avoidanceDirection * avoidanceWeight).normalized;

            agent.UpdateAgent(deltaTime, finalDirection);
        }
    }

    private Vector3 CalculateAvoidance(Agent agent)
    {
        Vector3 avoidance = Vector3.zero;

        foreach (Agent other in agents)
        {
            if (other != agent)
            {
                Vector3 toOther = agent.transform.position - other.transform.position;
                float distance = toOther.magnitude;

                if (distance < avoidanceRadius && distance > 0.01f) // Avoid division by zero
                {
                    avoidance += toOther.normalized / distance; // Stronger repulsion for closer agents
                }
            }
        }

        return avoidance.normalized; // Normalize to keep the vector magnitude consistent
    }
}
