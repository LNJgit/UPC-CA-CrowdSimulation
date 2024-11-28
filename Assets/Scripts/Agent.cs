using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Speed of movement
    public float directionChangeInterval = 2.0f; // Time in seconds to change direction
    [SerializeField] private float Minimum_distance_to_goal = 5.0f;

    private Vector3 currentDirection;
    public Vector3 Goal { get; private set; }
    public OrientationManager orientationManager;

    private void Start()
    {
        if (orientationManager == null)
        {
            Debug.LogError("OrientationManager is not assigned!");
        }

        AssignNewGoal(); // Assign an initial goal at the start
    }

    public void AssignNewGoal()
    {
        do
        {
            Goal = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        } while (Vector3.Distance(transform.position, Goal) < Minimum_distance_to_goal);

        Debug.Log($"{gameObject.name} assigned a new goal at {Goal}");
    }

    private void Update()
    {
        // Calculate direction toward the goal
        Vector3 direction = (Goal - transform.position).normalized;

        // Move and rotate the agent
        UpdateAgent(Time.deltaTime, direction);

        // Check if the agent has reached the goal
        if (Vector3.Distance(transform.position, Goal) < 0.5f)
        {
            Debug.Log($"{gameObject.name} reached its goal at {transform.position}");
            AssignNewGoal();
        }
    }

    public void UpdateAgent(float deltaTime, Vector3 direction)
    {
        // Move towards the goal
        Vector3 velocity = direction * moveSpeed * deltaTime;
        transform.Translate(velocity, Space.World);

        // Rotate towards the direction using OrientationManager
        orientationManager.Rotate(direction, moveSpeed);
    }
}
