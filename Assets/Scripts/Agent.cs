using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public Vector3 Goal { get; private set; }
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AssignNewGoal();
    }

    public void AssignNewGoal()
    {
        do
        {
            Goal = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        } while (Vector3.Distance(transform.position, Goal) < 1.0f);
    }

    public void UpdateAgent(float elapsedTime, Vector3 direction)
    {
        Vector3 velocity = direction * maxSpeed * elapsedTime;
        rb.MovePosition(rb.position + velocity);
    }
}
