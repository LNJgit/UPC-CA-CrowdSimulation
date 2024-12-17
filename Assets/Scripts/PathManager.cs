using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private Dictionary<Agent, Vector3> agentGoals = new Dictionary<Agent, Vector3>();
    public float neighborDistance = 3.0f; // Distance to check for neighboring agents

    public void RegisterAgent(Agent agent)
    {
        // Assign a new random goal to the agent
        agentGoals[agent] = GenerateRandomGoal();
    }

    public Vector3 GetGoalForAgent(Agent agent)
    {
        if (!agentGoals.ContainsKey(agent))
        {
            RegisterAgent(agent);
        }

        return agentGoals[agent];
    }

    public void UpdateGoalForAgent(Agent agent)
    {
        agentGoals[agent] = GenerateRandomGoal();
    }

    private Vector3 GenerateRandomGoal()
    {
        float worldLimit = 20.0f; // Adjust as per your scene's dimensions
        Vector3 goal = new Vector3(
            Random.Range(-worldLimit / 2, worldLimit / 2),
            0,
            Random.Range(-worldLimit / 2, worldLimit / 2)
        );

        return goal;
    }

}
