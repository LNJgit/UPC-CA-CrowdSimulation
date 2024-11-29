using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float maxSeeAhead = 2.0f; // Maximum distance to look ahead for obstacles
    public float avoidForce = 2.0f; // Force to apply for avoidance
    private Vector3 goal;
    private PathManager pathManager;
    public OrientationManager orientationManager;

    private void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
        goal = pathManager.GetGoalForAgent(this);
    }

    private void Update()
    {
        Vector3 direction = (goal - transform.position).normalized;
        Vector3 avoidance = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxSeeAhead))
        {
            if (hit.collider.CompareTag("Agent"))
            {
                avoidance = AvoidObstacle(hit, direction);
                direction += avoidance * avoidForce; // Modify direction based on avoidance
            }
        }

        UpdateAgent(Time.deltaTime, direction);

        if (Vector3.Distance(transform.position, goal) < 0.5f)
        {
            pathManager.UpdateGoalForAgent(this);
        }
    }

    private Vector3 AvoidObstacle(RaycastHit hit, Vector3 direction)
    {
        Vector3 avoidVector = Vector3.Reflect(direction, hit.normal); // Reflect direction based on normal
        return avoidVector.normalized;
    }

    public void UpdateAgent(float deltaTime, Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed * deltaTime;
        transform.Translate(velocity, Space.World);
        orientationManager.Rotate(direction, moveSpeed);
    }
}
