using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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
        goal = pathManager.GetGoalForAgent(this);
        Vector3 direction = (goal - transform.position);
        float distanceToGoal = direction.magnitude;
        direction = direction.normalized; // Normalize direction after calculating distance
        // Log the agent's current position, distance to the goal, and the goal's position
        Debug.Log(gameObject.name + " Position: " + transform.position + ", Distance to Goal: " + distanceToGoal + ", Goal Position: " + goal);

        if (distanceToGoal < 0.5f)
        {
            moveSpeed = 0.0f;
            StartCoroutine(UpdateGoal());
        }

        Vector3 avoidance = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxSeeAhead))
        {
            if (hit.collider.CompareTag("Agent"))
            {
                avoidance = AvoidObstacle(hit, direction);
                direction += avoidance * avoidForce;

                // Log avoidance action
                Debug.Log(gameObject.name + " avoiding " + hit.collider.gameObject.name + ", New Direction: " + direction);
            }
        }

        UpdateAgent(Time.deltaTime, direction);
    }


    private Vector3 AvoidObstacle(RaycastHit hit, Vector3 direction)
    {
        Vector3 avoidVector = Vector3.Reflect(direction, hit.normal); // Reflect direction based on normal
        return avoidVector.normalized;
    }

    private IEnumerator UpdateGoal()
    {
        yield return new WaitForSeconds(1);
        pathManager.UpdateGoalForAgent(this);
        moveSpeed = 1.0f;
    }

    public void UpdateAgent(float deltaTime, Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed * deltaTime;
        transform.Translate(velocity, Space.World);
        orientationManager.Rotate(direction, moveSpeed);
    }
}
