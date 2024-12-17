using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Agent : MonoBehaviour
{
    public float moveSpeed = 1.0f;
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
        direction = direction.normalized;

        // DRAW DEBUG LINE BETWEEN AGENT AND GOAL
        Debug.DrawLine(transform.position, goal, Color.blue);

        Debug.Log(gameObject.name + " Position: " + transform.position + ", Distance to Goal: " + distanceToGoal + ", Goal Position: " + goal);

        if (distanceToGoal < 0.5f)
        {
            moveSpeed = 0.0f;
            StartCoroutine(UpdateGoal());
        }

        UpdateAgent(Time.deltaTime, direction);
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
