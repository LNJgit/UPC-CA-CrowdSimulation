using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance { get; private set; }

    [SerializeField] private float goalThreshold = 0.5f;
    [SerializeField] private float avoidanceRadius = 5.0f;

    private List<Agent> agents = new List<Agent>();
    private float simulationTimeStep = 0.02f;

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
        StartCoroutine(SimulationLoop());
    }

    public void RegisterAgent(Agent agent)
    {
        if (!agents.Contains(agent))
        {
            agents.Add(agent);
        }
    }

    public void UnregisterAgent(Agent agent)
    {
        if (agents.Contains(agent))
        {
            agents.Remove(agent);
        }
    }

    private System.Collections.IEnumerator SimulationLoop()
    {
        while (true)
        {
            UpdateAgentPaths();
            yield return new WaitForSeconds(simulationTimeStep);
        }
    }

    public void UpdateAgentPaths()
    {
        foreach (Agent agent in agents)
        {
            if (Vector3.Distance(agent.transform.position, agent.Goal) <= goalThreshold)
            {
                agent.AssignNewGoal();
            }

            Vector3 avoidance = CalculateAvoidance(agent);
            Vector3 direction = ((agent.Goal - agent.transform.position).normalized * 0.7f) +
                                (avoidance.normalized * 0.3f);
            direction = direction.normalized;

            agent.UpdateAgent(simulationTimeStep, direction);
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

                if (distance < avoidanceRadius)
                {
                    avoidance += toOther.normalized / Mathf.Max(distance, 0.1f);
                }
            }
        }

        return Vector3.ClampMagnitude(avoidance, 1.0f);
    }
}
