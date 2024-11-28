using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float maxSpeed = 1.0f;
    public Vector3 Goal { get; private set; }

    private void Start()
    {
        PathManager.Instance.RegisterAgent(this);
        AssignNewGoal();
    }

    public void AssignNewGoal()
    {
        do
        {
            Goal = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        } while (Vector3.Distance(transform.position, Goal) < 1.0f);

        Debug.Log($"{gameObject.name} assigned a new goal at {Goal}");
    }

    public void UpdateAgent(float deltaTime, Vector3 direction)
    {
        Vector3 velocity = direction * maxSpeed * deltaTime;
        transform.Translate(velocity,Space.World);
        if (Vector3.Distance(transform.position, Goal) < 0.5f)
        {
            Debug.Log($"{gameObject.name} reached its goal at {transform.position}");
            AssignNewGoal();
        }
    }
}
